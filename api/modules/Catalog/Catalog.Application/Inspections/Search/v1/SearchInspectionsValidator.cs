using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public class SearchInspectionsValidator : AbstractValidator<SearchInspectionsCommand>
{
    public SearchInspectionsValidator()
    {
        // Only validate when both dates are provided
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("ToDate must be greater than or equal to FromDate.");
    }
}
