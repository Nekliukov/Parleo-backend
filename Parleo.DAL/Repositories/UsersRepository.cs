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

        public async Task<bool> AddAsync(UserInfo entity)
        {
            _context.UserInfo.Add(entity);
            var result =  await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = _context.UserInfo.First(user => user.Id == id);
            _context.UserInfo.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<IList<UserInfo>> GetAllAsync()
        {
            return await _context.UserInfo.ToListAsync();
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
