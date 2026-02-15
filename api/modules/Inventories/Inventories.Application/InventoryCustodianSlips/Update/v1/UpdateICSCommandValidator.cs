using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Update.v1;

public sealed class UpdateICSCommandValidator : AbstractValidator<UpdateICSCommand>
{
    public UpdateICSCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ICS ID is required");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.IssuanceDate)
            .NotEmpty().WithMessage("Issuance Date is required");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required");

        RuleForEach(x => x.LineItems)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.PropertyCode)
                    .NotEmpty().WithMessage("Property Code is required");

                item.RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");

                item.RuleFor(x => x.UnitCost)
                    .GreaterThan(0).WithMessage("Unit Cost must be greater than 0");
            });
    }
}
