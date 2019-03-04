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

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
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
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<UserInfo> GetUserByIdAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<UserInfo> CreateUserAsync(UserInfo user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await _repository.FindByEmailAsync(user.UserAuth.Email) != null)
                throw new AppException("Email \"" + user.UserAuth.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.UserAuth.PasswordHash = passwordHash;
            user.UserAuth.PasswordSalt = passwordSalt;

            var result = await _repository.AddAsync(user);
            if (!result)
                throw new AppException("Smth bad happens in the server side and user don't save");
            return user;
        }

        public async Task<bool> UpdateUserAsync(UserInfo userParam, string password = null)
        {
            var user = await _repository.GetAsync(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.UserAuth.Email != user.UserAuth.Email)
            {
                // Email has changed so check if the new Email is already taken
                if (await _repository.FindByEmailAsync(userParam.UserAuth.Email) != null)
                    throw new AppException("Email " + userParam.UserAuth.Email + " is already taken");
            }

            // update user properties
            user = userParam;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.UserAuth.PasswordHash = passwordHash;
                user.UserAuth.PasswordSalt = passwordSalt;
            }

            return await _repository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
