using Parleo.BLL.Interfaces;
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

namespace Parleo.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _repository;
        private readonly ISecurityHelper _securityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public AccountService(
            IUsersRepository repository,
            ISecurityHelper securityHelper,
            IMapperFactory mapperFactory,
            ILogger<AccountService> logger
        )
        {
            _repository = repository;
            _securityService = securityHelper;
            _mapper = mapperFactory.GetMapper(typeof(BLServices).Name);
            _logger = logger;
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

            LocationModel location = new LocationModel();
            (location.Latitude, location.Longitude) = (user.Latitude, user.Longitude);

            var usersPage = await _repository.GetPageAsync(
                _mapper.Map<UserFilter>(pageRequest), _mapper.Map<Location>(location));

            if(usersPage == null)
            {
                return null;
            }

            return _mapper.Map<PageModel<UserModel>>(usersPage);
        }

        public async Task<UserModel> GetUserByIdAsync(Guid id)
        {
            User user = await _repository.GetAsync(id);
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
    }
}
