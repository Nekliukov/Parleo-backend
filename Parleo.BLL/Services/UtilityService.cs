using AutoMapper;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.DAL.Interfaces;
using Parleo.DAL.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parleo.BLL.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IUtilityRepository _utilityRepository;
        private readonly IMapper _mapper;

        public UtilityService(
            IUtilityRepository utilityRepository, 
            IMapper mapper
        )
        {
            _utilityRepository = utilityRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync()
        {
            IReadOnlyCollection<Language> languages = await _utilityRepository.GetLanguagesAsync();
            return _mapper.Map<IReadOnlyCollection<Language>, IReadOnlyCollection<LanguageModel>>(languages);
        }

        public async Task<IReadOnlyCollection<HobbyModel>> GetHobbiesAsync()
        {
            IReadOnlyCollection<Hobby> hobbies = await _utilityRepository.GetHobbiesAsync();
            return _mapper.Map<IReadOnlyCollection<Hobby>, IReadOnlyCollection<HobbyModel>>(hobbies);
        }
    }
}
