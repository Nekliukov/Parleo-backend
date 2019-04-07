using Microsoft.AspNetCore.Mvc;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
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
        private readonly IConfiguration _configuration;
        private readonly IAccountImageSettings _accountImageSettings;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AccountController> logger,
            IAccountImageSettings accountImageSettings
        )
        {
            _accountService = accountService;
            _configuration = configuration; 
            _mapper = mapper;
            _logger = logger;
            _accountImageSettings = accountImageSettings;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsers(int offset)
        {
            if(offset < 0)
            {
                return BadRequest();
            }

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

        [HttpPut("account-image")]
        [Authorize]
        public async Task<IActionResult> AddUserAccountImage(IFormFile image)
        {
            string accountImagePath = _accountImageSettings.DestPath;
            Guid userId = new Guid(User.FindFirst(JwtRegisteredClaimNames.Jti).Value);
            UserModel user = await _accountService.GetUserByIdAsync(userId);

            if (user.AccountImage != null)
            {
                System.IO.File.Delete(Path.Combine(accountImagePath, user.AccountImage));
            }

            string accountImageUniqueId = Guid.NewGuid().ToString();
            string accountImageExtension = Path.GetExtension(image.FileName);
            string path = Path.Combine(accountImagePath, accountImageUniqueId + accountImageExtension);

            if (image != null && image.Length > 0)
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }

            await _accountService.InsertUserAccountImageAsync(
                accountImageUniqueId,
                userId
            );

            return Ok();
        }
    }
}
