using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Contexts;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;

namespace Parleo.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserContext _context;

        public UsersRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(UserInfo entity)
        {
            _context.UserInfo.Add(entity);
            var result =  await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DisableAsync(Guid id)
        {
            //TODO: update the IsEnabled field on the user when it appears on the model
            return true;
        }

        public async Task<IList<UserInfo>> GetPageAsync(int number)
        {
            //Hardcoded 25. add to configure, when it'll be necessary. This number was approved with front-end
            return await _context.UserInfo.Skip(number).Take(25).ToListAsync();
        }

        public async Task<UserInfo> GetAsync(Guid id)
        {
            return await _context.UserInfo
            .FirstOrDefaultAsync(user => user.Id == id);
        }


        public async Task<bool> UpdateAsync(UserInfo entity)
        {
            _context.UserInfo.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<UserAuth> FindByEmailAsync(string email)
        {
            return await _context.UserAuth.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
