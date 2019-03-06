using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<IList<UserInfo>> GetPageAsync(int number);

        Task<UserInfo> GetAsync(Guid id);

        Task<bool> CreateAsync(UserInfo entity);

        Task<bool> UpdateAsync(UserInfo entity);

        Task<bool> DisableAsync(Guid id);

        Task<UserAuth> FindByEmailAsync(string email);
    }
}