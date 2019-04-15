using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(AuthorizationModel authorizationModel);

        Task<PageModel<UserModel>> GetUsersPageAsync(UserFilterModel pageRequest);

        Task<UserModel> GetUserByIdAsync(Guid id);

        Task<UserModel> CreateUserAsync(AuthorizationModel authorizationModel);

        Task<bool> UpdateUserAsync(UserModel user);

        Task<bool> DisableUserAsync(Guid id);

        Task AddAccountToken(AccountTokenModel tokenModel);

        Task InsertUserAccountImageAsync(string imageName, Guid userId);

        Task<AccountTokenModel> DeleteAccountToken(Guid userId);
    }
}