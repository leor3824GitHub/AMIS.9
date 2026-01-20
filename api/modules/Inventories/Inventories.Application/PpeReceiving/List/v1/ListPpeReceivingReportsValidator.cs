using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed class ListPpeReceivingReportsValidator : AbstractValidator<ListPpeReceivingReportsQuery>
{
    public ListPpeReceivingReportsValidator()
    {
        // Query validation - currently no filters
    }
}
