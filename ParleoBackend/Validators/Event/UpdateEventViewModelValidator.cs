using AutoMapper;
using FluentValidation;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;
using System;
using System.Collections.Generic;

namespace ParleoBackend.Validators.Event
{
    public class UpdateEventViewModelValidator : AbstractValidator<UpdateEventViewModel>
    {
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;

        public UpdateEventViewModelValidator(IUtilityService utilityService, IMapper mapper)
        {
            _utilityService = utilityService;
            _mapper = mapper;

            RuleFor(ev => ev.Description).NotEmpty().MaximumLength(200);

            RuleFor(ev => ev.LanguageCode).NotNull().NotEmpty()
                .Must(LanguageExist).WithMessage(Constants.Errors.INVALID_LANGUAGE);

            RuleFor(ev => ev.Latitude)
                .InclusiveBetween(
                    Constants.Restrictions.MIN_LATITUDE_VALUE,
                    Constants.Restrictions.MAX_LATITUDE_VALUE)
                .WithMessage(Constants.Errors.INVALID_LOCATION);

            RuleFor(ev => ev.Longitude)
                .InclusiveBetween(
                    Constants.Restrictions.MIN_LONGITUDE_VALUE,
                    Constants.Restrictions.MAX_LONGITUDE_VALUE)
                .WithMessage(Constants.Errors.INVALID_LOCATION);

            RuleFor(ev => ev.MaxParticipants)
                .GreaterThanOrEqualTo(Constants.Restrictions.MIN_PARTICIPANTS_COUNT)
                .LessThanOrEqualTo(Constants.Restrictions.MAX_PARTICIPANTS_COUNT);

            RuleFor(ev => ev.Name).NotEmpty().MaximumLength(60);

            RuleFor(ev => ev.StartTime)
                .Must(CorrectStartDate)
                .WithMessage(Constants.Errors.INVALID_START_DATE);

            RuleFor(ev => ev.EndDate)
                .GreaterThan(ev => ev.StartTime)
                .LessThan(ev => ev.StartTime
                    .AddHours(Constants.Restrictions.MAX_HOURS_TO_END_DATE)
                    .AddMinutes(1))
                .WithMessage(Constants.Errors.INVALID_END_DATE);
        }

        private bool LanguageExist(string language)
        {
            var langToIEnumerable = new LanguageViewModel[]
            {
                new LanguageViewModel() {
                    Id = language
                }
            };

            return _utilityService.AllLanguagesExistAsync(
                _mapper.Map<ICollection<LanguageModel>>(langToIEnumerable)).Result;
        }

        private bool CorrectStartDate(DateTimeOffset dateTime)
        {
            return DateTime.Now < dateTime && dateTime < DateTime.Now.AddDays(
                Constants.Restrictions.MAX_DAYS_TO_START_DATE);
        }
    }
}
