using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Parleo.BLL.Interfaces;
using Parleo.BLL.Models.Entities;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Validators.User
{
    public class UpdateUserViewModelValidator : AbstractValidator<UpdateUserViewModel>
    {
        private readonly IUtilityService _utilityService;
        private readonly IMapper _mapper;

        public UpdateUserViewModelValidator(IUtilityService utilityService, IMapper mapper)
        {
            _utilityService = utilityService;
            _mapper = mapper;
            RuleFor(user => user.Birthdate).NotEmpty().NotNull()
                .Must(ValidBirthDate).WithMessage(Constants.Errors.INVALID_BIRTHDATE);
            RuleFor(user => user.Name).NotEmpty().NotNull()
                .MaximumLength(60);
            RuleFor(user => user.Languages).NotEmpty().NotNull()
                .Must(NoLanguageDuplicates).WithMessage(Constants.Errors.DUPLICATES_ARE_NOT_ALLOWED)
                .Must(AllLanguagesExist).WithMessage(Constants.Errors.INVALID_LANGUAGES);
            RuleFor(user => user.Languages)
                .Must(CorrectLevel).WithMessage(Constants.Errors.INCORRECT_LANGUAGE_LEVEL);
            RuleFor(user => user.Hobbies)
                .Must(AllHobbiesExist).WithMessage(Constants.Errors.INVALID_HOBBY);
        }

        private bool AllLanguagesExist(UserLanguageViewModel[] languages)
        {
            return _utilityService.AllLanguagesExistAsync(
                _mapper.Map<ICollection<LanguageModel>>(languages)).Result;
        }

        private bool NoLanguageDuplicates(UserLanguageViewModel[] languages)
        {
            return languages.Length == languages.Select(l => l.Code).Distinct().Count();
        }

        private bool AllHobbiesExist(IEnumerable<string> hobbies)
        {
            var hobbyList = hobbies.Select(hobby => new HobbyViewModel()
            {
                Name = hobby
            }).ToList();

            return _utilityService.AllHobbiesExistAsync(
                _mapper.Map<ICollection<HobbyModel>>(hobbyList)).Result;
        }

        private bool CorrectLevel(IEnumerable<UserLanguageViewModel> languages)
        {
            return languages.All(language => language.Level <= 
                Constants.Restrictions.MAX_LANGUAGE_LEVEL);
        }

        private bool ValidBirthDate(DateTime birthDate)
        {
            DateTime minDateTime = new DateTime(
                DateTime.Now.Year - Constants.Restrictions.MAX_AGE,
                DateTime.Now.Month,
                DateTime.Now.Day);

            DateTime maxDateTime = new DateTime(
                DateTime.Now.Year - Constants.Restrictions.MIN_AGE,
                DateTime.Now.Month,
                DateTime.Now.Day);

            return birthDate < maxDateTime && birthDate > minDateTime;
        }
    }
}
