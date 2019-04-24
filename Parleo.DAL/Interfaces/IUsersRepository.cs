using Parleo.DAL.Models.Entities;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Models.Pages;
using System;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<Page<User>> GetPageAsync(UserFilter userFilter, Location location);

        Task<User> GetAsync(Guid id);

        Task<bool> CreateAsync(User entity);

        Task<bool> UpdateAsync(User entity);

        Task<bool> DisableAsync(Guid id);

        Task<Credentials> FindByEmailAsync(string email);

        Task AddAccountTokenAsync(AccountToken accountToken);

        Task InsertAccountImageNameAsync(string imageName, Guid userId);

        Task<AccountToken> DeleteAccountTokenByUserIdAsync(Guid userId);
    }
}