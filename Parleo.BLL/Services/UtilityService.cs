using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Services
{
    public class UtilityService : IUtilityService
    {
        public UtilityService(

        )
        {

        }

        public async Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
