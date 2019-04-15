using FluentValidation;
using Parleo.BLL.Interfaces;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UserViewModelValidator: AbstractValidator<UserViewModel>
    {
        private readonly IAccountService _accountService;

        public UserViewModelValidator(IAccountService accountService)
        {
            _accountService = accountService;

            RuleFor(user => user.Birthdate).NotEmpty().NotNull();
            RuleFor(user => user.Email).NotEmpty().NotNull()
                .EmailAddress();
            RuleFor(user => user.Latitude).NotEmpty();
            RuleFor(user => user.Name).NotEmpty().NotNull()
                .MaximumLength(60);
        }
    }
}
