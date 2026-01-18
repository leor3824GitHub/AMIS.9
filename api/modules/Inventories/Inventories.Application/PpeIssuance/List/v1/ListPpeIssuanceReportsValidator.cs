using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;

public sealed class ListPpeIssuanceReportsValidator : AbstractValidator<ListPpeIssuanceReportsQuery>
{
    public ListPpeIssuanceReportsValidator()
    {
        // Query validation - can be empty for list all
    }
}
