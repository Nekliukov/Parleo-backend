using AutoMapper;
using Parleo.BLL.Extensions;
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
            IMapperFactory mapperFactory
        )
        {
            _utilityRepository = utilityRepository;
            _mapper = mapperFactory.GetMapper(typeof(BLServices).Name);
        }

        public async Task<IReadOnlyCollection<LanguageModel>> GetLanguagesAsync()
        {
            return _mapper.Map<IReadOnlyCollection<Language>, IReadOnlyCollection<LanguageModel>>(
                await _utilityRepository.GetLanguagesAsync()
            );
        }

        public async Task<IReadOnlyCollection<HobbyModel>> GetHobbiesAsync()
        {
            return _mapper.Map<IReadOnlyCollection<Hobby>, IReadOnlyCollection<HobbyModel>>(
                await _utilityRepository.GetHobbiesAsync()
            );
        }

        public async Task<bool> LanguageExistsAsync(ICollection<LanguageModel> languages) =>
            await _utilityRepository.IsLanguagesExistsAsync(_mapper.Map<ICollection<Language>>(languages));

        public async Task<bool> HobbiesExistsAsync(ICollection<HobbyModel> hobbies) =>
            await _utilityRepository.IsHobbiesExistsAsync(_mapper.Map<ICollection<Hobby>>(hobbies));
    }
}
