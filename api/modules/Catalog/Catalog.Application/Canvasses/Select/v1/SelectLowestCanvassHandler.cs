using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Select.v1;

public sealed class SelectLowestCanvassHandler(
    [FromKeyedServices("catalog:canvasses")] IRepository<Canvass> repository)
    : IRequestHandler<SelectLowestCanvassCommand, SelectLowestCanvassResponse>
{
    public async Task<SelectLowestCanvassResponse> Handle(SelectLowestCanvassCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load canvasses for the purchase request
        var spec = new SelectLowestCanvassSpecs(request.PurchaseRequestId);
        var canvasses = await repository.ListAsync(spec, cancellationToken);
        if (canvasses.Count == 0)
        {
            throw new NotFoundException($"No canvasses found for PurchaseRequest {request.PurchaseRequestId}.");
        }

        // Determine lowest quoted price (tie-breaker: earliest ResponseDate)
        var lowest = canvasses
            .OrderBy(c => c.QuotedPrice)
            .ThenBy(c => c.ResponseDate)
            .First();

        // Update selection flags
        foreach (var c in canvasses)
        {
            if (c.Id == lowest.Id)
            {
                c.MarkAsSelected();
            }
            else if (c.IsSelected)
            {
                c.UnmarkAsSelected();
            }
            // Repository likely tracks entities; no explicit update needed per entity.
        }

        await repository.SaveChangesAsync(cancellationToken);

        return new SelectLowestCanvassResponse(lowest.Id, lowest.QuotedPrice);
    }
}
