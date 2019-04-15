using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Net;
using Parleo.BLL;
using ParleoBackend.Contracts;
using ParleoBackend.ViewModels.Filters;
using Parleo.BLL.Models.Filters;
using ParleoBackend.ViewModels.Pages;
using Parleo.BLL.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using ParleoBackend.Contracts;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IAccountImageSettings _accountImageSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IJwtService jwtService,
            IEmailService emailService
            IConfiguration configuration,
            ILogger<AccountController> logger,
            IAccountImageSettings accountImageSettings
        )
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _accountImageSettings = accountImageSettings;
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

            string tokenString = _jwtService.GetJWTToken(user);
            await _accountService.AddAccountToken(
                new AccountTokenModel()
                {
                    ExpirationDate = new DateTime().AddHours(2),
                    UserId = user.Id
                }
            );
            await _emailService.SendEmailConfirmationLink(user.Email, tokenString);
            return NoContent();
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

            string tokenString = _jwtService.GetJWTToken(user);
            return Ok(new {token = tokenString});
        }

        [HttpPut("{userId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
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

        [HttpGet("{userId}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            UserModel user = await _accountService.GetUserByIdAsync(new Guid(userId));
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<UserViewModel>(user));
        }


        [HttpGet("activate")]
        public async Task<IActionResult> GetActivatedUserAccount(string token)
        {
            string userIdString = _jwtService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userIdString))
            {
                return BadRequest();
            }


            UserModel user = await _accountService.GetUserByIdAsync(new Guid(userIdString));

            return Ok(user);
        }


        
        [HttpPut("{userId}/image")]
        [Authorize]
        public async Task<IActionResult> AddUserAccountImage(IFormFile image)
        {
            if (image == null)
            {
                return BadRequest();
            }

            string accountImagePath = _accountImageSettings.DestPath;
            Guid userId = new Guid(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);
            UserModel user = await _accountService.GetUserByIdAsync(userId);

            if (user.AccountImage != null)
            {
                System.IO.File.Delete(Path.Combine(accountImagePath, user.AccountImage));
            }

            string accountImageUniqueName = await image.SaveAsync(accountImagePath);

            await _accountService.InsertUserAccountImageAsync(
                accountImageUniqueName,
                userId
            );

            return Ok();
        }
    }
}
