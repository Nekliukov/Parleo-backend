using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Parleo.BLL.Extensions;
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
            RuleFor(user => user.Birthdate).NotEmpty().NotNull();
            RuleFor(user => user.Name).NotEmpty().NotNull()
                .MaximumLength(60);
            RuleFor(user => user.Languages).NotEmpty().NotNull()
                .Must(ExistsLanguages).WithMessage(Constants.Errors.INVALID_LANGUAGE);
            RuleFor(user => user.Languages)
                .Must(CorrectLevel).WithMessage(Constants.Errors.INCORRECT_LANGUAGE_LEVEL);
            RuleFor(user => user.Hobbies)
                .Must(ExistsHobbies).WithMessage(Constants.Errors.INVALID_HOBBY);
        }

        private bool ExistsLanguages(UserLanguageViewModel[] languages) =>
            _utilityService.LanguageExistsAsync(_mapper.Map<ICollection<LanguageModel>>(languages)).Result;

        private bool ExistsHobbies(IEnumerable<string> hobbies)
        {
            var hobbyList = hobbies.Select(hobby => new HobbyViewModel() {Name = hobby}).ToList();
            return _utilityService.HobbiesExistsAsync(_mapper.Map<ICollection<HobbyModel>>(hobbyList)).Result;
        }

        private bool CorrectLevel(IEnumerable<UserLanguageViewModel> languages)
        {
            return languages.All(language => language.Level <= 5);
        }
    }
}
