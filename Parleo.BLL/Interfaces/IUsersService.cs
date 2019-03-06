using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<UserAuth> AuthenticateAsync(string email, string password);

        Task<IEnumerable<UserInfo>> GetUsersPageAsync(int number);

        Task<UserInfo> GetUserByIdAsync(Guid id);

        Task<UserInfo> CreateUserAsync(UserInfo user, string password);

        Task<bool> UpdateUserAsync(UserInfo user, string password = null);

        Task<bool> DisableUserAsync(Guid id);

    }
}