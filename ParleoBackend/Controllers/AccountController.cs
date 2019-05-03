using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Net;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.ViewModels.Filters;
using Parleo.BLL.Models.Filters;
using ParleoBackend.ViewModels.Pages;
using System.Linq;
using ParleoBackend.Validators.User;
using FluentValidation.Results;
using Parleo.BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Parleo.BLL.Extensions;
using ParleoBackend.Validators.Common;
using ParleoBackend.Services;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IImageSettings _accountImageSettings;
        private readonly IJwtSettings _jwtSettings;
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(
            IAccountService accountService,
            IMapperFactory mapperFactory,
            IJwtService jwtService,
            IEmailService emailService,
            ILogger<AccountController> logger,
            IImageSettings accountImageSettings,
            IUtilityService utilityService,
            IJwtSettings jwtSettings
        )
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _mapper = mapperFactory.GetMapper(typeof(WebServices).Name);
            _logger = logger;
            _emailService = emailService;
            _accountImageSettings = accountImageSettings;
            _utilityService = utilityService;
            _jwtSettings = jwtSettings;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsersPageAsync(
            [FromQuery] UserFilterViewModel userFilter)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if(id == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            var validator = new PageRequestViewModelValidator();
            ValidationResult validationResult = validator.Validate(userFilter);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(
                    validationResult.Errors.First().ErrorMessage));
            }

            var currentUser = await _accountService.GetUserByIdAsync(new Guid(id));
            var users = await _accountService.GetUsersPageAsync(
                _mapper.Map<UserFilterModel>(userFilter), new Guid(id));

            if (users == null)
            {
                return NotFound();
            }

            foreach(var listUser in users.Entities)
            {
                listUser.DistanceFromCurrentUser = await _accountService
                    .GetDistanceFromCurrentUserAsync(currentUser.Id, listUser.Id);
            }

            return Ok(_mapper.Map<PageViewModel<UserViewModel>>(users));
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

        [HttpPut("current")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> EditAsync(
            [FromBody] UpdateUserViewModel entity)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (!Guid.TryParse(id, out Guid userGuid))
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.WRONG_GUID_FORMAT));
            }

            var validator = new UpdateUserViewModelValidator(_utilityService, _mapper);
            ValidationResult result = validator.Validate(entity);

            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            bool isEdited = await _accountService.UpdateUserAsync(
                    userGuid, _mapper.Map<UpdateUserModel>(entity));

            if (!isEdited)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            return NoContent();
        }

        [HttpGet("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId)
        {
            UserModel user = await _accountService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            user.DistanceFromCurrentUser = await _accountService
                .GetDistanceFromCurrentUserAsync(new Guid(id), user.Id);

            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpGet("me")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserByTokenAsync()
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (id == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }
            UserModel user = await _accountService.GetUserByIdAsync(new Guid(id));
            if (user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            return Ok(_mapper.Map<UserViewModel>(user));
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

            AccountTokenModel accountToken = await _accountService.DeleteAccountTokenAsync(userId);
            if (accountToken == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.EXPIRED_TOKEN));
            }

            UserModel user = await _accountService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.EXPIRED_TOKEN));
            }

            return Ok(new {
                id = user.Id,
                token = _jwtService.GetJWTToken(user, new ClaimsService(_jwtSettings))
            });
        }

        [HttpPut("current/image")]
        public async Task<IActionResult> AddUserAccountImage(IFormCollection formData)
        {
            if (formData == null)
            {
                return BadRequest();
            }

            IFormFile image = formData.Files.GetFile("Image");

            if (image == null)
            {
                return BadRequest();
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (!Guid.TryParse(id, out Guid userGuid))
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }
            
            UserModel user = await _accountService.GetUserByIdAsync(userGuid);

            string accountImagePath = _accountImageSettings.AccountDestPath;
            if (user.AccountImage != null)
            {
                System.IO.File.Delete(Path.Combine(accountImagePath, user.AccountImage));
            }

            string accountImageUniqueName = await image.SaveAsync(accountImagePath);

            FileInfo file = new FileInfo(Path.Combine(accountImagePath, accountImageUniqueName));
            file.OptimizeImage();

            await _accountService.InsertUserAccountImageAsync(
                accountImageUniqueName,
                userGuid
            );

            return Ok();
        }

        [HttpPut("current/location")]
        public async Task<IActionResult> UpdateUserLocation([FromBody] LocationViewModel location)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (!Guid.TryParse(id, out Guid userGuid))
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            var validator = new LocationViewModelValidator();
            ValidationResult result = validator.Validate(location);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            bool isEdited = await _accountService.UpdateUserLocationAsync(userGuid, _mapper.Map<LocationModel>(location));

            if (!isEdited)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            return NoContent();
        }
    }
}
