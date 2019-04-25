using FluentValidation;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.Common
{
    public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
    {
        public LocationViewModelValidator()
        {
            RuleFor(location => location.Latitude)
                .InclusiveBetween(-90, 90).WithMessage(Constants.Errors.INVALID_LOCATION);
            RuleFor(location => location.Longitude)
                .InclusiveBetween(-180, 180).WithMessage(Constants.Errors.INVALID_LOCATION);
        }
    }
}
