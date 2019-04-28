using Microsoft.EntityFrameworkCore;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parleo.DAL.Repositories
{
    public class UtilityRepository: IUtilityRepository
    {
        private readonly AppContext _context;

        public UtilityRepository(AppContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Language>> GetLanguagesAsync()
        {
            return await _context.Language.ToListAsync();
        }

        public async Task<IReadOnlyCollection<Hobby>> GetHobbiesAsync()
        {
            return await _context.Hobby.Include(hobby => hobby.Category).ToListAsync();
        }

        public async Task<bool> IsLanguagesExistsAsync(ICollection<Language> languages)
        {
            return await _context.Language.CountAsync(l => languages.Any(lan => lan.Code == l.Code)) == languages.Count;
        }

        public async Task<bool> IsHobbiesExistsAsync(ICollection<Hobby> hobbies)
        {
            return await _context.Hobby.CountAsync(hobby => hobbies.Any(h=> h.Name == hobby.Name)) == hobbies.Count;
        }
    }
}
