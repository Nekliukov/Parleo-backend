using FluentValidation;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.Event
{
    public class CreateEventViewModelValidator: AbstractValidator<CreateEventViewModel>
    {
        public CreateEventViewModelValidator()
        {
            RuleFor(ev => ev.Description).NotEmpty().MaximumLength(200);
            RuleFor(ev => ev.LanguageCode).NotNull().NotEmpty();
            RuleFor(ev => ev.Latitude).NotNull().NotEmpty();
            RuleFor(ev => ev.Longitude).NotNull().NotEmpty();
            RuleFor(ev => ev.MaxParticipants)
                .GreaterThanOrEqualTo(1)
                .LessThan(30);
            RuleFor(ev => ev.Name).NotEmpty();
        }
    }
}
