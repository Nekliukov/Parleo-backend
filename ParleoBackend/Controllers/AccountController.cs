using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using AutoMapper;

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
        public void Post(AuthorizationRequest authorizationRequest)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationRequest);
            _accountService.CreateUserAsync(authorizationModel);
        }
    }
}
