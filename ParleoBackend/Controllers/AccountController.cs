using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using AutoMapper;
using System.Threading.Tasks;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            IMapper mapper
        )
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AuthorizationRequest authorizationRequest)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationRequest);
            await _accountService.CreateUserAsync(authorizationModel);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthorizationRequest authorizationRequest)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationRequest);
            await _accountService.AuthenticateAsync(authorizationModel);
            return Ok();
        }
    }
}
