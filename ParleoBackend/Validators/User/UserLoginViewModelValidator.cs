using FluentValidation;
using Parleo.BLL.Interfaces;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UserLoginViewModelValidator: AbstractValidator<UserLoginViewModel>
    {
        private readonly IAccountService _accountService;

        public UserLoginViewModelValidator(IAccountService accountService)
        {
            _accountService = accountService;

            RuleFor(user => user.Email)
                .NotEmpty().NotNull()
                .EmailAddress()
                .Must(IsExists).WithMessage(Constants.Errors.EMAIL_NOT_FOUND);

            RuleFor(user => user.Password).
                NotEmpty().NotNull()
                .MinimumLength(8);
        }

        private bool IsExists(string email) => _accountService.UserExistsAsync(email).Result;
    }
}
