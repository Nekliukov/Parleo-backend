using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models;
using ParleoBackend.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using ParleoBackend.Extensions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Net;
using Parleo.BLL;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AccountController> logger
        )
        {
            _accountService = accountService;
            _configuration = configuration; 
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUsers(int offset)
        {
            IEnumerable<UserModel> users = await _accountService.GetUsersPageAsync(offset);
            if (users == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterAsync(AuthorizationViewModel authorizationViewModel)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationViewModel);
            UserModel user;
            try
            {
                user = await _accountService.CreateUserAsync(authorizationModel);
            }
            catch(AppException ex)
            {
                return BadRequest(ex.Error.ToString());
            }

            if (user == null)
            {
                return BadRequest();
            }

            string tokenString = AuthorizationExtension.GetJWTToken(user, _configuration.GetSection("JWTSecretKey").Value);
            return Ok(new { token = tokenString });
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync(AuthorizationViewModel authorizationViewModel)
        {
            AuthorizationModel authorizationModel = _mapper.Map<AuthorizationModel>(authorizationViewModel);
            UserModel user;
            try
            {
                user = await _accountService.AuthenticateAsync(authorizationModel);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Error.ToString());
            }
 
            string tokenString = AuthorizationExtension.GetJWTToken(user, _configuration.GetSection("JWTSecretKey").Value);

            return Ok(new { token = tokenString });
        }

        [HttpPut("edit")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditAsync(UserViewModel user)
        {
            bool isEdited = false;
            try
            {
                isEdited = await _accountService.UpdateUserAsync(_mapper.Map<UserModel>(user));
            }
            catch(AppException ex)
            {
                return BadRequest(ex.Error.ToString());
            }

            if (!isEdited)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet("getUser")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserAsync()
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            UserModel user = await _accountService.GetUserByIdAsync(new Guid(id));
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<UserViewModel>(user));
        }
    }
}
