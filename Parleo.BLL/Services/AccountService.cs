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
            IMapper mapper,
            ILogger<AccountService> logger
        )
        {
            _repository = repository;
            _securityService = securityHelper;
            _mapper = mapper;
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
            UserFilterModel pageRequest)
        {
            var usersPage = await _repository.GetPageAsync(
                _mapper.Map<UserFilter>(pageRequest));

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

        public async Task<bool> UpdateUserAsync(UserModel user)
        {
            User User = await _repository.GetAsync(user.Id);

            if (User == null)
            {
                return false; //bad request
            }            

            User = _mapper.Map<User>(user);

            return await _repository.UpdateAsync(User);
        }

        public async Task<bool> DisableUserAsync(Guid id)
            => await _repository.DisableAsync(id);

        public async Task<bool> IsUserExists(string email)
            => await _repository.FindByEmailAsync(email) != null;

        public async Task InsertUserAccountImageAsync(string imageName, Guid userId)
            => await _repository.InsertAccountImageNameAsync(imageName, userId);
            
        }
    }
}
