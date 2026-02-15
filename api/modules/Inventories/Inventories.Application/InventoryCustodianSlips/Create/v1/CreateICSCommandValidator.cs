using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Create.v1;

public sealed class CreateICSCommandValidator : AbstractValidator<CreateICSCommand>
{
    public CreateICSCommandValidator()
    {
        RuleFor(x => x.ICSNumber)
            .NotEmpty().WithMessage("ICS Number is required");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required");

        RuleFor(x => x.IssuanceDate)
            .NotEmpty().WithMessage("Issuance Date is required");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required")
            .Must(x => x.Count > 0).WithMessage("At least one line item is required");

        RuleForEach(x => x.LineItems)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.PropertyCode)
                    .NotEmpty().WithMessage("Property Code is required");

                item.RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0");

                item.RuleFor(x => x.DateAcquired)
                    .NotEmpty().WithMessage("Date Acquired is required");

                item.RuleFor(x => x.UnitCost)
                    .GreaterThan(0).WithMessage("Unit Cost must be greater than 0");
            });
    }
}
