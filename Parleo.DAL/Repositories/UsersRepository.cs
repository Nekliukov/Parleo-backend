using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Contexts;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;

namespace Parleo.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Contexts.AppContext _context;

        public UsersRepository(Contexts.AppContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(User entity)
        {
            _context.User.Add(entity);
            var result =  await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DisableAsync(Guid id)
        {
            //TODO: update the IsEnabled field on the user when it appears on the model
            return true;
        }

        public async Task<IList<User>> GetPageAsync(int offset)
        {
            //Hardcoded 25. add to configure, when it'll be necessary. This number was approved with front-end
            return await _context.User.Skip(offset).Take(25).Include(u => u.Credentials).ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _context.User.Include(u => u.Credentials)
                .FirstOrDefaultAsync(user => user.Id == id);
        }


        public async Task<bool> UpdateAsync(User entity)
        {
            _context.User.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<Credentials> FindByEmailAsync(string email)
        {
            return await _context.Credentials.Include(c => c.User).FirstOrDefaultAsync(с => с.Email == email);
        }
    }
}
