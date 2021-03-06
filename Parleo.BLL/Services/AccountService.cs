﻿using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using Parleo.DAL.Models.Entities;
using Parleo.DAL.Interfaces;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Parleo.BLL.Models.Filters;
using Parleo.BLL.Models.Pages;
using Parleo.DAL.Models.Filters;
using Parleo.BLL.Extensions;
using Parleo.DAL.Helpers;
using System.Linq;
using Parleo.DAL.Models.Pages;

namespace Parleo.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _repository;
        private readonly ISecurityHelper _securityService;
        private readonly IMapper _mapper;


        public AccountService(
            IUsersRepository repository,
            ISecurityHelper securityHelper,
            IMapperFactory mapperFactory
        )
        {
            _repository = repository;
            _securityService = securityHelper;
            _mapper = mapperFactory.GetMapper(typeof(BLServices).Name);
        }

        public async Task<UserModel> AuthenticateAsync(UserLoginModel authorizationModel)
        {
            Credentials user = await _repository.FindByEmailAsync(authorizationModel.Email);
                
            if (!_securityService.VerifyPasswordHash(authorizationModel.Password,
                user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
                
            return _mapper.Map<UserModel>(user.User);
        }

        public async Task<PageModel<UserModel>> GetUsersPageAsync(
            UserFilterModel pageRequest, Guid userId)
        {
            User user = await _repository.GetAsync(userId);
            if(user == null)
            {
                return null;
            }

            var usersPage = await _repository.GetPageAsync(
                _mapper.Map<UserFilter>(pageRequest), user);

            if (usersPage == null)
            {
                return null;
            }

            PageModel<UserModel> page = _mapper.Map<PageModel<UserModel>>(usersPage);

            foreach (UserFriends userFriend in user.Friends)
            {
                UserModel userModel = page.Entities.FirstOrDefault(u => u.Id == userFriend.UserToId);
                if (userModel != null && userFriend.Status == (int)FriendStatus.InFriends)
                {
                    userModel.IsFriend = true;
                }
            }

            UserModel currentUser = _mapper.Map<UserModel>(user);
            foreach (var listUser in page.Entities)
            {
                listUser.DistanceFromCurrentUser = GetDistanceFromCurrentUserAsync(currentUser, listUser);
            }

            return page;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id, Guid currentUserId)
        {
            User user = await _repository.GetAsync(id);
            User currentUser = await _repository.GetAsync(currentUserId);

            if (user == null || currentUser == null)
            {
                return null;
            }

            UserModel result = _mapper.Map<UserModel>(user);

            result.IsFriend = currentUser.Friends.Any(fr =>
                fr.UserToId == result.Id && fr.Status == (int)FriendStatus.InFriends
            );

            result.DistanceFromCurrentUser = GetDistanceFromCurrentUserAsync(
                _mapper.Map<UserModel>(currentUser),
                result
            );

            return result;
        }

        public async Task<UserModel> GetUserByIdAsync(Guid currentUserId)
        {
            User user = await _repository.GetAsync(currentUserId);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> CreateUserAsync(UserRegistrationModel authorizationModel)
        {
            byte[] passwordHash, passwordSalt;
            _securityService.CreatePasswordHash(authorizationModel.Password, out passwordHash, out passwordSalt);

            User user = new User()
            {
                Credentials = _mapper.Map<Credentials>(authorizationModel)
            };
            
            user.Credentials.PasswordHash = passwordHash;
            user.Credentials.PasswordSalt = passwordSalt;

            var result = await _repository.CreateAsync(user);
            if (!result)
            {
                return null;
            }

            return _mapper.Map<UserModel>(user);
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserModel user)
        {
            User User = await _repository.GetAsync(userId);

            if (User == null)
            {
                return false; //bad request
            }            

            _mapper.Map(user, User);

            return await _repository.UpdateAsync(User);
        }

        public async Task<bool> UpdateUserLocationAsync(Guid userId, LocationModel location)
        {
            User user = await _repository.GetAsync(userId);

            if (user == null)
            {
                return false;
            }

            (user.Latitude, user.Longitude) = (location.Latitude, location.Longitude);

            return await _repository.UpdateAsync(user);
        }

        public async Task AddAccountTokenAsync(AccountTokenModel tokenModel)
        {
            await _repository.AddAccountTokenAsync(_mapper.Map<AccountToken>(tokenModel));
        }

        public async Task<AccountTokenModel> DeleteAccountTokenAsync(Guid userId)
        {
            return _mapper.Map<AccountTokenModel>(await _repository.DeleteAccountTokenByUserIdAsync(userId));
        }

        public async Task<int> GetDistanceFromCurrentUserAsync(Guid mainUserId, Guid targetUserId)
        {
            User mainUser = await _repository.GetAsync(mainUserId);
            User targetUser = await _repository.GetAsync(targetUserId);
            double resultDistance = LocationHelper.GetDistanceBetween((double)mainUser.Longitude, (double)mainUser.Latitude,
                (double)targetUser.Longitude, (double)targetUser.Latitude);

            return (int)Math.Round(resultDistance);
        }

        private int GetDistanceFromCurrentUserAsync(UserModel mainUser, UserModel targetUser)
        {
            double resultDistance = LocationHelper.GetDistanceBetween((double)mainUser.Longitude, (double)mainUser.Latitude,
                (double)targetUser.Longitude, (double)targetUser.Latitude);

            return (int)Math.Round(resultDistance);
        }

        public async Task<bool> DisableUserAsync(Guid id)
            => await _repository.DisableAsync(id);

        public async Task<bool> UserExistsAsync(string email)
            => await _repository.FindByEmailAsync(email) != null;

        public async Task InsertUserAccountImageAsync(string imageName, Guid userId)
            => await _repository.InsertAccountImageNameAsync(imageName, userId);

        public async Task ClearExpiredTokensAsync()
        {
            await _repository.ClearExpiredAccountTokensAsync();
        }

        public async Task<bool> CheckUserHasTokenAsync(string email)
        {
            return await _repository.CheckUserHasTokenAsync(email);
        }

        public async Task<bool> AddFriendAsync(Guid userFromId, Guid userToId)
        {
            if (Equals(userFromId, userToId))
            {
                return false;
            }

            if(await _repository.GetAsync(userFromId) == null)
            {
                return false;
            }

            return await _repository.AddFriendAsync(userFromId, userToId);
        }

        public async Task<PageModel<UserModel>> GetUserFriendsAsync(PageRequestModel pageRequest, Guid userId)
        {
            var friends = await _repository.GetUserFriendsAsync(_mapper.Map<PageRequest>(pageRequest), userId);
            if (friends == null)
            {
                return null;
            }

            PageModel<UserModel> friendsPage = _mapper.Map<PageModel<UserModel>>(friends);

            foreach (UserModel user in friendsPage.Entities)
            {
                user.IsFriend = true;
            }

            return friendsPage;
        }

        public async Task<bool> RemoveFriendAsync(Guid userFromId, Guid userToId)
        {
            if (userToId == null)
            {
                return false;
            }

            if (Equals(userFromId, userToId))
            {
                return false;
            }

            if (await _repository.GetAsync(userFromId) == null)
            {
                return false;
            }

            return await _repository.RemoveFriendAsync(userFromId, userToId);
        }
    }
}
