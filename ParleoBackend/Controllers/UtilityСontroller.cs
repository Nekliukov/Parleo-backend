using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Extensions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilityController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;

        public UtilityController(
            IUtilityService utilityService,
            IMapperFactory mapperFactory
        )
        {
            _utilityService = utilityService;
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
        }

        [HttpGet("languages")]
        public async Task<IReadOnlyCollection<LanguageViewModel>> GetLanguagesAsync()
        {
            IReadOnlyCollection<LanguageModel> languages = await _utilityService.GetLanguagesAsync();
            return _mapper.Map<IReadOnlyCollection<LanguageModel>, IReadOnlyCollection<LanguageViewModel>>(languages);
        }

        [HttpGet("hobbies")]
        public async Task<IReadOnlyCollection<HobbyViewModel>> GetHobbiesAsync()
        {
            IReadOnlyCollection<HobbyModel> hobbies = await _utilityService.GetHobbiesAsync();
            return _mapper.Map<IReadOnlyCollection<HobbyModel>, IReadOnlyCollection<HobbyViewModel>>(hobbies);
        }
    }
}
