using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parleo.BLL.Exceptions;
using Parleo.BLL.Extensions;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Validators.Common;
using ParleoBackend.Validators.User;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Filters;
using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEventService _eventService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IImageSettings _accountImageSettings;
        private readonly IJwtSettings _jwtSettings;
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UsersController(
            IAccountService accountService,
            IEventService eventService,
            IMapperFactory mapperFactory,
            IJwtService jwtService,
            IEmailService emailService,
            ILogger<AccountsController> logger,
            IImageSettings accountImageSettings,
            IUtilityService utilityService,
            IJwtSettings jwtSettings
        )
        {
            _accountService = accountService;
            _eventService = eventService;
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
            if (id == null)
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

            foreach (var listUser in users.Entities)
            {
                listUser.DistanceFromCurrentUser = await _accountService
                    .GetDistanceFromCurrentUserAsync(currentUser.Id, listUser.Id);
            }

            return Ok(_mapper.Map<PageViewModel<UserViewModel>>(users));
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

        [HttpGet("current")]
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

            FileExtension.OptimizeImage(Path.Combine(accountImagePath, accountImageUniqueName));

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

        [HttpGet("current/created-events")]
        public async Task<IActionResult> GetCreatedEvents(
            [FromQuery] PageRequestViewModel pageRequest)
        {
            var validator = new PageRequestViewModelValidator();
            ValidationResult validationResult = validator.Validate(pageRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(
                    validationResult.Errors.First().ErrorMessage));
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var createdEvents = await _eventService.GetCreatedEvents(new Guid(id),
                _mapper.Map<PageRequestModel>(pageRequest));

            return Ok(_mapper.Map<PageViewModel<EventViewModel>>(createdEvents));
        }

        [HttpGet("current/attending-events")]
        public async Task<IActionResult> GetAttendingEvents(
            [FromQuery] PageRequestViewModel pageRequest)
        {
            var validator = new PageRequestViewModelValidator();
            ValidationResult validationResult = validator.Validate(pageRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(
                    validationResult.Errors.First().ErrorMessage));
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            var attendingEvents = await _eventService.GetAttendingEvents(new Guid(id),
                _mapper.Map<PageRequestModel>(pageRequest));

            return Ok(_mapper.Map<PageViewModel<EventViewModel>>(attendingEvents));
        }

        [HttpPut("current/removeFriend/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveFriend(Guid userId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (id == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            Guid userFromId = new Guid(id);
            bool isRemoved = false;

            if (userFromId != null)
            {
                isRemoved = await _accountService.RemoveFriendAsync(userFromId, userId);
            }
            else
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            if (!isRemoved)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.FRIEND_DELETE_FAILED));
            }

            return NoContent();
        }

        [HttpGet("current/friends")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetUserFriends([FromQuery] PageRequestViewModel pageRequest)
        {
            var validator = new PageRequestViewModelValidator();
            ValidationResult result = validator.Validate(pageRequest);
            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponseFormat(result.Errors.First().ErrorMessage));
            }

            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (id == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            var friends = await _accountService.GetUserFriendsAsync(
                _mapper.Map<PageRequestModel>(pageRequest), new Guid(id));

            return Ok(_mapper.Map<PageViewModel<UserViewModel>>(friends));
        }

        [HttpPut("current/friends/{userToId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> SendFriendshipRequest(Guid userToId)
        {
            string id = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (id == null)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            Guid userFromId = new Guid(id);
            bool isSent = false;

            if (userFromId != null)
            {
                isSent = await _accountService.AddFriendAsync(userFromId, userToId);
            }
            else
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.USER_NOT_FOUND));
            }

            if (!isSent)
            {
                return BadRequest(new ErrorResponseFormat(Constants.Errors.FRIEND_REQUSET_FAILED));
            }

            return NoContent();
        }
    }
}
