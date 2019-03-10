using Parleo.BLL.Models;
using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserAuth> AuthenticateAsync(AuthorizationModel authorizationModel);

        Task<IEnumerable<UserInfoModel>> GetUsersPageAsync(int number);

        Task<UserInfoModel> GetUserByIdAsync(Guid id);

        Task<UserInfoModel> CreateUserAsync(AuthorizationModel authorizationModel);

        Task<bool> UpdateUserAsync(UserInfoModel user);

        Task<bool> DisableUserAsync(Guid id);

    }
}