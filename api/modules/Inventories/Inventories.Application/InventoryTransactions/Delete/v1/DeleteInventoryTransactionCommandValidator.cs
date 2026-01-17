using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactions.Delete.v1;

public class DeleteInventoryTransactionCommandValidator : AbstractValidator<DeleteInventoryTransactionCommand>
{
    public DeleteInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

