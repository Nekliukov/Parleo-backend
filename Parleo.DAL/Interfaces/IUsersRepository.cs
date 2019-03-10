using Parleo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.DAL.Interfaces
{
    public interface IUsersRepository
    {
        Task<IList<User>> GetPageAsync(int number);

        Task<User> GetAsync(Guid id);

        Task<bool> CreateAsync(User entity);

        Task<bool> UpdateAsync(User entity);

        Task<bool> DisableAsync(Guid id);

        Task<Credentials> FindByEmailAsync(string email);
    }
}