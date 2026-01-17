using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.Acceptances.ManageItems.v1;

public sealed class UpdateAcceptanceItemHandler(
    [FromKeyedServices("inventories:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<UpdateAcceptanceItemCommand>
{
    public async Task Handle(UpdateAcceptanceItemCommand request, CancellationToken cancellationToken)
    {
        var acceptance = await repository.GetByIdAsync(request.AcceptanceId, cancellationToken);
        if (acceptance is null)
            throw new Exception($"Acceptance {request.AcceptanceId} not found");

        acceptance.UpdateItem(request.ItemId, request.QtyAccepted, request.Remarks);
        await repository.UpdateAsync(acceptance, cancellationToken);
    }
}

