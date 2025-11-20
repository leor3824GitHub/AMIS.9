using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Update.v1;

public sealed class UpdateCanvassHandler(
    [FromKeyedServices("catalog:canvasses")] IRepository<Canvass> repository)
    : IRequestHandler<UpdateCanvassCommand, UpdateCanvassResponse>
{
    public async Task<UpdateCanvassResponse> Handle(UpdateCanvassCommand request, CancellationToken cancellationToken)
    {
        var canvass = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Canvass with ID {request.Id} not found.");

        canvass.Update(
            request.ItemDescription,
            request.Quantity,
            request.Unit,
            request.QuotedPrice,
            request.Remarks,
            request.ResponseDate);

        await repository.UpdateAsync(canvass, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateCanvassResponse(canvass.Id);
    }
}
