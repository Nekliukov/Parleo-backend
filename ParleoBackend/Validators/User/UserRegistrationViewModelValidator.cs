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

            RuleFor(user => user.ConfirmedPassword)
                .NotNull().NotEmpty()
                .MinimumLength(8)
                .Equal(user => user.Password).WithMessage(Constants.Errors.PASSWORDS_DO_NOT_MATCH);
        }

        private bool IsUnique(string email) => !_accountService.IsUserExists(email).Result;
    }
}
