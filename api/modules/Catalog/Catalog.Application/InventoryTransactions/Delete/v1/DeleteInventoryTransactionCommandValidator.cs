using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InventoryTransactions.Delete.v1;

public class DeleteInventoryTransactionCommandValidator : AbstractValidator<DeleteInventoryTransactionCommand>
{
    public DeleteInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
