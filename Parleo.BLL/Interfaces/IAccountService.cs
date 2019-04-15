using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.BLL.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> AuthenticateAsync(UserLoginModel authorizationModel);

        Task<PageModel<UserModel>> GetUsersPageAsync(UserFilterModel pageRequest);

        Task<UserModel> GetUserByIdAsync(Guid id);

        Task<UserModel> CreateUserAsync(UserRegistrationModel authorizationModel);

        Task<bool> UpdateUserAsync(Guid userId, UpdateUserModel user);

        Task<bool> DisableUserAsync(Guid id);

        Task<bool> IsUserExists(string email);
      
        Task InsertUserAccountImageAsync(string imageName, Guid userId);
    }
}