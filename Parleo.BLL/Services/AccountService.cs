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

        public async Task<UserAuth> AuthenticateAsync(AuthorizationModel authorizationModel)
        {
            if (string.IsNullOrEmpty(authorizationModel.Email) || string.IsNullOrEmpty(authorizationModel.Password))
                return null;

            var user = await _repository.FindByEmailAsync(authorizationModel.Password);

            // check if email exists
            if (user == null)
                return null;

            if (!_securityService.VerifyPasswordHash(authorizationModel.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        //TODO: add filters
        public async Task<IEnumerable<UserInfoModel>> GetUsersPageAsync(int offset)
        {
            IList<UserInfo> users = await _repository.GetPageAsync(offset);
            return _mapper.Map<IEnumerable<UserInfoModel>>(users);
        }

        public async Task<UserInfoModel> GetUserByIdAsync(Guid id)
        {
            UserInfo user = await _repository.GetAsync(id);
            return _mapper.Map<UserInfoModel>(user);
        }

        public async Task<UserInfoModel> CreateUserAsync(AuthorizationModel authorizationModel)
        {
            // validation
            if (string.IsNullOrWhiteSpace(authorizationModel.Password))
                throw new AppException(ErrorType.InvalidPassword, "Password is required");

            if (await _repository.FindByEmailAsync(authorizationModel.Email) != null)
                throw new AppException(ErrorType.ExistingEmail,"Email \"" + authorizationModel.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            _securityService.CreatePasswordHash(authorizationModel.Password, out passwordHash, out passwordSalt);

            UserInfo user = _mapper.Map<UserInfo>(authorizationModel);

            user.UserAuth.PasswordHash = passwordHash;
            user.UserAuth.PasswordSalt = passwordSalt;

            var result = await _repository.CreateAsync(user);
            if (!result)
                throw new Exception("Smth bad happend on the server side and user wasn't saved");

            return _mapper.Map<UserInfoModel>(user);
        }

        public async Task<bool> UpdateUserAsync(UserInfoModel user)
        {
            UserInfo userInfo = await _repository.GetAsync(user.Id);

            if (userInfo == null)
                throw new AppException(ErrorType.InvalidId, "User not found");

            if (user.Email != userInfo.UserAuth.Email)
            {
                // Email has changed so check if the new Email is already taken
                if (await _repository.FindByEmailAsync(user.Email) != null)
                    throw new AppException(ErrorType.ExistingEmail, "Email " + user.Email + " is already taken");
            }

            // update user properties
            userInfo = _mapper.Map<UserInfo>(user);

            return await _repository.UpdateAsync(userInfo);
        }

        public async Task<bool> DisableUserAsync(Guid id)
        {
            return await _repository.DisableAsync(id);
        }
    }
}
