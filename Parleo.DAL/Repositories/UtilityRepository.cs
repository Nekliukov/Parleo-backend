using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using System;
using System.Collections.Generic;
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

        public Task<IReadOnlyCollection<Language>> GetLanguagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
