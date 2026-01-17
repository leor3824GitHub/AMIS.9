using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactions.Update.v1;

public class UpdateInventoryTransactionCommandValidator : AbstractValidator<UpdateInventoryTransactionCommand>
{
    public UpdateInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
        RuleFor(x => x.UnitCost).GreaterThan(0);
        RuleFor(x => x.TransactionType).IsInEnum();
    }
}

