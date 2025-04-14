using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.CreateBulk.v1;
public class CreateBulkPurchaseItemCommandValidator : AbstractValidator<CreateBulkPurchaseItemCommand>
{
    public CreateBulkPurchaseItemCommandValidator()
    {       
        RuleFor(p => p.Items).NotEmpty();
    }
}
