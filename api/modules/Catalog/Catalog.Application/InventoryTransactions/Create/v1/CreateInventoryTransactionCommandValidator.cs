using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Create.v1;

public class CreateInventoryTransactionCommandValidator : AbstractValidator<CreateInventoryTransactionCommand>
{
    public CreateInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
        RuleFor(x => x.UnitCost).GreaterThan(0);
        //RuleFor(x => x.TransactionType).IsInEnum();
    }
}

