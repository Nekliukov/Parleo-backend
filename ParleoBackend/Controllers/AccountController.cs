using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers(int offset)
        {
            IEnumerable<UserInfoModel> users = await _accountService.GetUsersPageAsync(offset);
            return Ok(_mapper.Map<IEnumerable<UserInfoViewModel>>(users));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(AuthorizationViewModel authorizationRequest)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationRequest);
            UserInfoModel user = await _accountService.CreateUserAsync(authorizationModel);
            return Ok(_mapper.Map<UserInfoViewModel>(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthorizationViewModel authorizationRequest)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationRequest);
            await _accountService.AuthenticateAsync(authorizationModel);
            return Ok();
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> EditAsync(UserInfoViewModel user)
        {
            if (await _accountService.UpdateUserAsync(_mapper.Map<UserInfoModel>(user)))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            // jwt
            return NoContent();
        }

        [HttpGet("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            // get id from jwt token
            UserInfoModel user = await _accountService.GetUserByIdAsync(new Guid());
            return Ok(_mapper.Map<UserInfoViewModel>(user));
        }
    }
}
