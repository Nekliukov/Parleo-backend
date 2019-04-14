using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using ParleoBackend.Extensions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Net;
using ParleoBackend.ViewModels.Filters;
using Parleo.BLL.Models.Filters;
using ParleoBackend.ViewModels.Pages;
using System.Linq;
using ParleoBackend.Validators.User;
using FluentValidation.Results;
using Parleo.BLL.Exceptions;

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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsersPageAsync(
            [FromQuery] UserFilterViewModel userFilter)
        {
            var users = await _accountService.GetUsersPageAsync(
                _mapper.Map<UserFilterModel>(userFilter));

            if (users == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PageViewModel<UserViewModel>>(users));
        }

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterAsync(UserRegistrationViewModel registrationViewModel)
        {
            var validator = new UserRegistrationViewModelValidator(_accountService);
            ValidationResult result = validator.Validate(registrationViewModel);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            UserRegistrationModel authorizationModel = _mapper.Map<UserRegistrationModel>(registrationViewModel);
            UserModel user = await _accountService.CreateUserAsync(authorizationModel);
            if (user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_CREATION_FAILED));
            }

            string tokenString = AuthorizationExtension.GetJWTToken(user,_configuration.GetSection("JWTSecretKey").Value);
            return Ok(new { token = tokenString });
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync(UserLoginViewModel loginViewModel)
        {
            var validator = new UserLoginViewModelValidator(_accountService);
            ValidationResult result = validator.Validate(loginViewModel);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            UserLoginModel authorizationModel = _mapper.Map<UserLoginModel>(loginViewModel);
            UserModel user = await _accountService.AuthenticateAsync(authorizationModel);
            if(user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.INVALID_PASSWORD));
            }
                
            string tokenString = AuthorizationExtension.GetJWTToken(user, _configuration.GetSection("JWTSecretKey").Value);
            return Ok(new { token = tokenString });
        }

        [HttpPut("edit")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> EditAsync(UserViewModel user)
        {
            var validator = new UserViewModelValidator(_accountService);
            ValidationResult result = validator.Validate(user);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            bool isEdited = false;
            isEdited = await _accountService.UpdateUserAsync(_mapper.Map<UserModel>(user));          
            if (!isEdited)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
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
