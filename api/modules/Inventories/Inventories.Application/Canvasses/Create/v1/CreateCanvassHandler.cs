using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Create.v1;

public sealed class CreateCanvassHandler(
    [FromKeyedServices("inventories:canvasses")] IRepository<Canvass> repository)
    : IRequestHandler<CreateCanvassCommand, CreateCanvassResponse>
{
    public async Task<CreateCanvassResponse> Handle(CreateCanvassCommand request, CancellationToken cancellationToken)
    {
        var canvass = Canvass.Create(
            request.PurchaseRequestId,
            request.SupplierId,
            request.ItemDescription,
            request.Quantity,
            request.Unit,
            request.QuotedPrice,
            request.Remarks,
            request.ResponseDate ?? DateTime.UtcNow);

        await repository.AddAsync(canvass, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateCanvassResponse(canvass.Id);
    }
}

