using System;
using System.Collections.Generic;

namespace Parleo.DAL.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private UserContext _context;
        public UsersRepository(UserContext context)
        {
            _context = context;
        }
        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete (Guid id)
        {
            var entity = _context.Users.First(user => user.Key == key);
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public IList<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User Get(Guid id)
        {
            return _context.Users
            .FirstOrDefault(x => x.UserID == id);
        }


        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
