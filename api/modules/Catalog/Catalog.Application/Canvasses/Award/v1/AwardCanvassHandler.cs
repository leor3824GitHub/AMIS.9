using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Ardalis.Specification;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Award.v1;

internal sealed class OtherCanvassesForPurchaseRequestSpecs : Specification<Canvass>
{
    public OtherCanvassesForPurchaseRequestSpecs(Guid purchaseRequestId, Guid excludeId)
    {
        Query.Where(c => c.PurchaseRequestId == purchaseRequestId && c.Id != excludeId);
    }
}

public sealed class AwardCanvassHandler(
    [FromKeyedServices("catalog:canvasses")] IRepository<Canvass> repository)
    : IRequestHandler<AwardCanvassCommand, AwardCanvassResponse>
{
    public async Task<AwardCanvassResponse> Handle(AwardCanvassCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var canvass = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (canvass is null)
        {
            throw new NotFoundException($"Canvass {request.Id} not found.");
        }

        // Load other canvasses for same purchase request and unselect them.
        var othersSpec = new OtherCanvassesForPurchaseRequestSpecs(canvass.PurchaseRequestId, canvass.Id);
        var others = await repository.ListAsync(othersSpec, cancellationToken);
        foreach (var other in others)
        {
            if (other.IsSelected)
            {
                other.UnmarkAsSelected();
            }
        }

        // Mark chosen as selected.
        canvass.MarkAsSelected();

        await repository.SaveChangesAsync(cancellationToken);

        return new AwardCanvassResponse(canvass.Id, canvass.SupplierId, canvass.QuotedPrice);
    }
}
