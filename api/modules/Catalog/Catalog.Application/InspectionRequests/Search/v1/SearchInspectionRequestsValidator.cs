using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public class SearchInspectionRequestsValidator : AbstractValidator<SearchInspectionRequestsCommand>
{
    public SearchInspectionRequestsValidator()
    {
        // Enforce that when both dates are provided, ToDate >= FromDate
        RuleFor(x => x)
            .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.ToDate!.Value >= x.FromDate!.Value)
            .WithMessage("ToDate must be greater than or equal to FromDate.");
    }
}
