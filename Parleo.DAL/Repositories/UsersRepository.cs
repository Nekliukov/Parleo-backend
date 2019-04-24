using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Filters;
using Parleo.DAL.Models.Pages;
using Parleo.DAL.Helpers;

namespace Parleo.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppContext _context;

        private readonly int _defaultPageSize = 25;

        public UsersRepository(AppContext context)
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

        public async Task<Page<User>> GetPageAsync(UserFilter userFilter, Location location)
        {
            double latitude = (double)location.Latitude,
                   longtitude = (double)location.Longitude;
            var users = await _context.User
                .Where(u => userFilter.Gender != null ?
                    u.Gender == userFilter.Gender : true)
                .Where(u => (userFilter.Languages != null &&
                        userFilter.Languages.Count() != 0) ?
                    userFilter.Languages.Any(fl => u.Languages.Any(
                        ul => ul.LanguageCode == fl.LanguageCode &&
                            LevelInRange(fl, ul))) : true)
                .Where(u => (userFilter.MaxDistance != null) ?
                    LocationHelper.GetDistanceBetween((double)u.Longitude, (double)u.Latitude,
                    longtitude, latitude) <= userFilter.MaxDistance : true)
                .Where(u => (userFilter.MaxAge != null) ?
                    GetAge(u.Birthdate) <= userFilter.MaxAge : true)
                .Where(u => (userFilter.MinAge != null) ?
                    GetAge(u.Birthdate) >= userFilter.MinAge : true)
                .Include(u => u.Languages)
                    .ThenInclude(ul => ul.Language)
                .Include(u => u.CreatedEvents)
                .Include(u => u.Friends)
                    .ThenInclude(f => f.UserTo)
                .Include(u => u.Credentials)
                .Include(u => u.Hobbies)
                    .ThenInclude(h => h.Hobby)
                .ToListAsync();

            int totalAmount = users.Count();

            if (userFilter.PageSize == null)
            {
                userFilter.PageSize = _defaultPageSize;
            }

            return new Page<User>()
            {
                Entities = users
                    .Skip((userFilter.Page - 1) * userFilter.PageSize.Value)
                    .Take(userFilter.PageSize.Value).ToList(),
                PageNumber = userFilter.Page,
                PageSize = userFilter.PageSize.Value,
                TotalAmount = totalAmount
            };
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _context.User.Include(u => u.Credentials)
                .Include(u => u.Languages)
                .Include(u => u.Hobbies)
                    .ThenInclude(uh => uh.Hobby)
                        .ThenInclude(h => h.Category)
                .Include(u => u.Friends)
                .Include(u => u.CreatedEvents)
                .Include(u => u.AttendingEvents)
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
            return await _context.Credentials.Include(c => c.User)
                .FirstOrDefaultAsync(с => с.Email == email);
        }

        public async Task AddAccountTokenAsync(AccountToken accountToken)
        {
            _context.AccountToken.Add(accountToken);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountToken> DeleteAccountTokenByUserIdAsync(Guid userId)
        {
            AccountToken accountToken = await _context.AccountToken.Include(c => c.User)
                .FirstOrDefaultAsync(с => с.UserId == userId);
            _context.AccountToken.Remove(accountToken);
            _context.SaveChanges();

            return accountToken;
        }

        public async Task InsertAccountImageNameAsync(string imageName, Guid userId)
        {
            User user = new User()
            {
                Id = userId,
                AccountImage = imageName
            };
            _context.Entry(user).Property(x => x.AccountImage).IsModified = true;
            await _context.SaveChangesAsync();
        }


        private int GetAge(DateTimeOffset birth)
        {
            int age = new DateTime(
                DateTime.Now.Subtract(birth.DateTime).Ticks).Year;

            return DateTime.Now.DayOfYear < birth.DayOfYear ? age - 1 : age;
        }

        private bool LevelInRange(
            FilteringLanguage filteringLanguage, UserLanguage userLanguage)
        {
            bool lessOrEqualThanMax = filteringLanguage.MaxLevel == null ?
                userLanguage.Level <= filteringLanguage.MaxLevel : true;

            return lessOrEqualThanMax && filteringLanguage.MinLevel == null ?
                userLanguage.Level >= filteringLanguage.MinLevel : true;
        }
    }
}
