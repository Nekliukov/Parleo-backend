﻿using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Parleo.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _repository;
        private readonly ISecurityHelper _securityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public AccountService(
            IUsersRepository repository,
            ISecurityHelper securityHelper,
            IMapper mapper,
            ILogger<AccountService> logger
        )
        {
            _repository = repository;
            _securityService = securityHelper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserModel> AuthenticateAsync(AuthorizationModel authorizationModel)
        {
            Credentials user = await _repository.FindByEmailAsync(authorizationModel.Email);
     
            if (user == null) // check if email exists
            {
                throw new AppException(ErrorType.EmailNotFound,
                    $"{authorizationModel.Email} is not found");
            }
                
            if (!_securityService.VerifyPasswordHash(authorizationModel.Password,
                user.PasswordHash, user.PasswordSalt))
            {
                throw new AppException(ErrorType.InvalidPassword,
                    $"Wrong password for {authorizationModel.Email}");
            }
                
            return _mapper.Map<UserModel>(user.User);
        }

        //TODO: add filters
        public async Task<IEnumerable<UserModel>> GetUsersPageAsync(int offset)
        {
            IList<User> users = await _repository.GetPageAsync(offset);
            if(users == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            User user = await _repository.GetAsync(id);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(AuthorizationModel authorizationModel)
        {
            if (await _repository.FindByEmailAsync(authorizationModel.Email) != null)
            {
                throw new AppException(ErrorType.ExistingEmail, $"Email {authorizationModel.Email} is already exists");
            }

            byte[] passwordHash, passwordSalt;
            _securityService.CreatePasswordHash(authorizationModel.Password, out passwordHash, out passwordSalt);

            User user = new User()
            {
                Credentials = _mapper.Map<Credentials>(authorizationModel)
            };
            
            user.Credentials.PasswordHash = passwordHash;
            user.Credentials.PasswordSalt = passwordSalt;

            var result = await _repository.CreateAsync(user);
            if (!result)
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<bool> UpdateUserAsync(UserModel user)
        {
            User User = await _repository.GetAsync(user.Id);

            if (User == null)
            {
                return false; //bad request
            }
                
            if (user.Email != User.Credentials.Email)
            {
                if (await _repository.FindByEmailAsync(user.Email) != null)
                {
                    throw new AppException(ErrorType.ExistingEmail,
                        "Email " + user.Email + " is already taken");
                }                 
            }

            User = _mapper.Map<User>(user);

            return await _repository.UpdateAsync(User);
        }

        public async Task<bool> DisableUserAsync(Guid id)
        {
            return await _repository.DisableAsync(id);
        }

        public async Task AddAcountToken(AccountTokenModel tokenModel)
        {
            await _repository.AddAccountToken(_mapper.Map<AccountToken>(tokenModel));
        }
    }
}
