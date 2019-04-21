using FluentValidation;
using Parleo.BLL.Interfaces;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserViewModelValidator()
        {
            RuleFor(user => user.Birthdate).NotEmpty().NotNull();
            RuleFor(user => user.Name).NotEmpty().NotNull()
                .MaximumLength(60);
        }
    }
}
