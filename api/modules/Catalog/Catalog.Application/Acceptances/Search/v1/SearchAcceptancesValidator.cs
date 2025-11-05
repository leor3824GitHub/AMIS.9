using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;

public class SearchAcceptancesValidator : AbstractValidator<SearchAcceptancesCommand>
{
    public SearchAcceptancesValidator()
    {
        // Enforce that when both dates are provided, ToDate >= FromDate
        RuleFor(x => x)
            .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.ToDate!.Value >= x.FromDate!.Value)
            .WithMessage("ToDate must be greater than or equal to FromDate.");
    }
}
