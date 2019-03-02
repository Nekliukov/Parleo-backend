using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Contexts;
using Parleo.DAL.Entities;
using Parleo.DAL.Interfaces;

namespace Parleo.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public UserAuth AddAsync(UserAuth entity)
        {
            _context.UserAuths.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public void DeleteAsync(Guid id)
        {
            var entity = _context.UserAuths.First(user => user.UserInfoId == id);
            _context.UserAuths.Remove(entity);
            _context.SaveChanges();
        }

        public UserAuth FindByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<UserAuth> GetAllAsync()
        {
            return _context.UserAuths.ToList();
        }

        public UserAuth GetAsync(Guid id)
        {
            return _context.UserAuths
            .FirstOrDefault(x => x.UserInfoId == id);
        }

        public void UpdateAsync(UserAuth entity)
        {
            _context.UserAuths.Update(entity);
            _context.SaveChanges();
        }
    }
}
