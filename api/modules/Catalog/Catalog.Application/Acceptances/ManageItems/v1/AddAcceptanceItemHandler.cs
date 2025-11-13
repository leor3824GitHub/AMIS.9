using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Acceptances.ManageItems.v1;

public sealed class AddAcceptanceItemHandler(
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<AddAcceptanceItemCommand, AddAcceptanceItemResponse>
{
    public async Task<AddAcceptanceItemResponse> Handle(AddAcceptanceItemCommand request, CancellationToken cancellationToken)
    {
        var acceptance = await repository.GetByIdAsync(request.AcceptanceId, cancellationToken);
        if (acceptance is null)
            throw new Exception($"Acceptance {request.AcceptanceId} not found");

        acceptance.AddItem(request.PurchaseItemId, request.QtyAccepted, request.Remarks);
        var item = acceptance.Items.Last();
        await repository.UpdateAsync(acceptance, cancellationToken);
        return new AddAcceptanceItemResponse(item.Id);
    }
}
