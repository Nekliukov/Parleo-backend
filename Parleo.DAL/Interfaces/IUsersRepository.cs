using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<IList<UserInfo>> GetAllAsync();

        Task<UserInfo> GetAsync(Guid id);

        Task<bool> AddAsync(UserInfo entity);

        Task<bool> UpdateAsync(UserInfo entity);

        Task<bool> DeleteAsync(Guid id);

        Task<UserAuth> FindByEmailAsync(string email);
    }
}