using FluentValidation;
using Parleo.BLL.Interfaces;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        private readonly IAccountService _accountService;

        public UpdateUserViewModelValidator(IAccountService accountService)
        {
            _accountService = accountService;

            RuleFor(user => user.Birthdate).NotEmpty().NotNull();
            RuleFor(user => user.Email).NotEmpty().NotNull()
                .EmailAddress();
            RuleFor(user => user.Latitude).NotNull().NotEmpty();
            RuleFor(user => user.Longitude).NotNull().NotEmpty();
            RuleFor(user => user.Name).NotEmpty().NotNull()
                .MaximumLength(60);
        }
    }
}
