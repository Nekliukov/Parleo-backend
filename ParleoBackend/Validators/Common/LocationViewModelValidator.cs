using FluentValidation;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.Common
{
    public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
    {
        public LocationViewModelValidator()
        {
            RuleFor(location => location.Latitude)
                .InclusiveBetween(
                    Constants.Restrictions.MIN_LATITUDE_VALUE, 
                    Constants.Restrictions.MAX_LATITUDE_VALUE)
                .WithMessage(Constants.Errors.INVALID_LOCATION);
            RuleFor(location => location.Longitude)
                .InclusiveBetween(
                    Constants.Restrictions.MIN_LONGITUDE_VALUE,
                    Constants.Restrictions.MAX_LONGITUDE_VALUE)
                .WithMessage(Constants.Errors.INVALID_LOCATION);
        }
    }
}
