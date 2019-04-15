using FluentValidation;
using Parleo.BLL.Interfaces;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UserRegistrationViewModelValidator: AbstractValidator<UserRegistrationViewModel>
    {
        private readonly IAccountService _accountService;

        public UserRegistrationViewModelValidator(IAccountService accountService)
        {
            _accountService = accountService;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(user => user.Email)
            .NotEmpty().NotNull()
            .EmailAddress()
            .Must(IsUnique).WithMessage(Constants.Errors.EMAIL_ALREADY_EXISTS);

            RuleFor(user => user.Password)
                .NotNull().NotEmpty()
                .MinimumLength(8);           
        }

        private bool IsUnique(string email) => !_accountService.IsUserExistsAsync(email).Result;
    }
}
