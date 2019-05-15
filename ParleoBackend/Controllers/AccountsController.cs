using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net;
using ParleoBackend.Contracts;
using System.Linq;
using ParleoBackend.Validators.User;
using FluentValidation.Results;
using Parleo.BLL.Exceptions;
using Parleo.BLL.Extensions;
using ParleoBackend.Services;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IJwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public AccountsController(
            IAccountService accountService,
            IMapperFactory mapperFactory,
            IJwtService jwtService,
            IEmailService emailService,
            IJwtSettings jwtSettings
        )
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
            _emailService = emailService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("register")]
        [AllowAnonymous]
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

            string tokenString = _jwtService.GetJWTToken(user, new EmailClaimsService(_jwtSettings));
            await _accountService.AddAccountTokenAsync(
                new AccountTokenModel()
                {
                    ExpirationDate = DateTime.Now.AddMinutes(10),
                    UserId = user.Id
                }
            );
            await _emailService.SendEmailConfirmationLinkAsync(user.Email, tokenString);

            return NoContent();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAsync(UserLoginViewModel loginViewModel)
        {
            var validator = new UserLoginViewModelValidator(_accountService);
            ValidationResult result = validator.Validate(loginViewModel);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            if (await _accountService.CheckUserHasTokenAsync(loginViewModel.Email))
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.TOKEN_ALREADY_SENT));
            }

            UserLoginModel authorizationModel = _mapper.Map<UserLoginModel>(loginViewModel);
            UserModel user = await _accountService.AuthenticateAsync(authorizationModel);
            if(user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.INVALID_PASSWORD));
            }
            string tokenString = _jwtService.GetJWTToken(user, new ClaimsService(_jwtSettings));

            return Ok(new {token = tokenString});
        }

        [HttpGet("activate")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetActivatedUserAccount(string token)
        {
            string userIdString = _jwtService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userIdString))
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.NOT_VALID_TOKEN));
            }

            Guid userId = new Guid(userIdString);

            UserModel user = await _accountService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.EXPIRED_TOKEN));
            }

            AccountTokenModel accountToken = await _accountService.DeleteAccountTokenAsync(userId);
            if (accountToken == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.EXPIRED_TOKEN));
            }

            return Ok(new {
                id = user.Id,
                token = _jwtService.GetJWTToken(user, new ClaimsService(_jwtSettings))
            });
        }
    }
}
