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
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;

        public UtilitiesController(
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
            return _mapper.Map<IReadOnlyCollection<LanguageModel>, IReadOnlyCollection<LanguageViewModel>>(
                await _utilityService.GetLanguagesAsync()
            );
        }

        [HttpGet("hobbies")]
        public async Task<IReadOnlyCollection<HobbyViewModel>> GetHobbiesAsync()
        {
            return _mapper.Map<IReadOnlyCollection<HobbyModel>, IReadOnlyCollection<HobbyViewModel>>(
                await _utilityService.GetHobbiesAsync()
            );
        }
    }
}
