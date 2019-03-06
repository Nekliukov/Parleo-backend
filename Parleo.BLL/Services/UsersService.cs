using Parleo.BLL.Helpers;
using Parleo.BLL.Interfaces;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parleo.BLL.Services
{
    public class UsersService : IUsersService
    {
        private IUsersRepository _repository;
        private readonly SecurityHelper _securityHelper;

        public UsersService(IUsersRepository repository, SecurityHelper securityHelper)
        {
            _repository = repository;
            _securityHelper = securityHelper;
        }
        public async Task<UserAuth> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _repository.FindByEmailAsync(email);

            // check if email exists
            if (user == null)
                return null;

            // check if password is correct
            if (!_securityHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        //TODO: add filters
        public async Task<IEnumerable<UserInfo>> GetUsersPageAsync(int number)
        {
            return await _repository.GetPageAsync(number);
        }

        public async Task<UserInfo> GetUserByIdAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException(ErrorType.InvalidPassword, "Password is required");

            if (await _repository.FindByEmailAsync(user.UserAuth.Email) != null)
                throw new AppException(ErrorType.ExistingEmail,"Email \"" + user.UserAuth.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            _securityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.UserAuth.PasswordHash = passwordHash;
            user.UserAuth.PasswordSalt = passwordSalt;

            var result = await _repository.CreateAsync(user);
            if (!result)
                throw new Exception("Smth bad happend on the server side and user wasn't saved");
            return user;
        }

        public async Task<bool> UpdateUserAsync(UserInfo user, string password = null)
        {
            var userInfo = await _repository.GetAsync(user.Id);

            if (userInfo == null)
                throw new AppException(ErrorType.InvalidId, "User not found");

            if (user.UserAuth.Email != userInfo.UserAuth.Email)
            {
                // Email has changed so check if the new Email is already taken
                if (await _repository.FindByEmailAsync(user.UserAuth.Email) != null)
                    throw new AppException(ErrorType.ExistingEmail, "Email " + user.UserAuth.Email + " is already taken");
            }

            // update user properties
            userInfo = user;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                _securityHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                userInfo.UserAuth.PasswordHash = passwordHash;
                userInfo.UserAuth.PasswordSalt = passwordSalt;
            }

            return await _repository.UpdateAsync(userInfo);
        }

        public async Task<bool> DisableUserAsync(Guid id)
        {
            return await _repository.DisableAsync(id);
        }
    }
}
