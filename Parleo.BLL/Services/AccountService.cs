using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace Parleo.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _repository;
        private readonly ISecurityHelper _securityService;
        private readonly IMapper _mapper;


        public AccountService(
            IUsersRepository repository,
            ISecurityHelper securityHelper,
            IMapper mapper
        )
        {
            _repository = repository;
            _securityService = securityHelper;
            _mapper = mapper;
        }

        public async Task<Credentials> AuthenticateAsync(AuthorizationModel authorizationModel)
        {
            if (string.IsNullOrEmpty(authorizationModel.Email) || string.IsNullOrEmpty(authorizationModel.Password))
                return null;

            var user = await _repository.FindByEmailAsync(authorizationModel.Email);

            // check if email exists
            if (user == null)
                return null;

            if (!_securityService.VerifyPasswordHash(authorizationModel.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        //TODO: add filters
        public async Task<IEnumerable<UserModel>> GetUsersPageAsync(int offset)
        {
            IList<User> users = await _repository.GetPageAsync(offset);
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            User user = await _repository.GetAsync(id);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(AuthorizationModel authorizationModel)
        {
            // validation
            if (string.IsNullOrWhiteSpace(authorizationModel.Password))
                throw new AppException(ErrorType.InvalidPassword, "Password is required");

            if (await _repository.FindByEmailAsync(authorizationModel.Email) != null)
                throw new AppException(ErrorType.ExistingEmail,"Email \"" + authorizationModel.Email + "\" is already taken");

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
                throw new Exception("Smth bad happend on the server side and user wasn't saved");

            return _mapper.Map<UserModel>(user);
        }

        public async Task<bool> UpdateUserAsync(UserModel user)
        {
            User User = await _repository.GetAsync(user.Id);

            if (User == null)
                throw new AppException(ErrorType.InvalidId, "User not found");

            if (user.Email != User.Credentials.Email)
            {
                // Email has changed so check if the new Email is already taken
                if (await _repository.FindByEmailAsync(user.Email) != null)
                    throw new AppException(ErrorType.ExistingEmail, "Email " + user.Email + " is already taken");
            }

            // update user properties
            User = _mapper.Map<User>(user);

            return await _repository.UpdateAsync(User);
        }

        public async Task<bool> DisableUserAsync(Guid id)
        {
            return await _repository.DisableAsync(id);
        }
    }
}
