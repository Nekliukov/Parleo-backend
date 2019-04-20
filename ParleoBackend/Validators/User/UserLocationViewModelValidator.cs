using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UserLocationViewModelValidator : AbstractValidator<UserLocationViewModel>
    {
        public UserLocationViewModelValidator()
        {
            RuleFor(user => user.Latitude).NotNull().NotEmpty();
            RuleFor(user => user.Longitude).NotNull().NotEmpty();
        }
    }
}
