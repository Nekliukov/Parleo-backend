using FluentValidation;
using ParleoBackend.ViewModels.Pages;

namespace ParleoBackend.Validators.Common
{
    public class PageRequestViewModelValidator : AbstractValidator<PageRequestViewModel>
    {
        public PageRequestViewModelValidator()
        {
            RuleFor(pageRequest => pageRequest.PageNumber)
                .GreaterThan(0);
            RuleFor(pageRequest => pageRequest.PageSize)
                .InclusiveBetween(
                Constants.Restrictions.MIN_PAGE_SIZE, 
                Constants.Restrictions.MAX_PAGE_SIZE);
        }
    }
}
