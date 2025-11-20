using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Delete.v1;

public sealed class DeleteCanvassHandler(
    [FromKeyedServices("catalog:canvasses")] IRepository<Canvass> repository)
    : IRequestHandler<DeleteCanvassCommand, DeleteCanvassResponse>
{
    public async Task<DeleteCanvassResponse> Handle(DeleteCanvassCommand request, CancellationToken cancellationToken)
    {
        var canvass = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Canvass with ID {request.Id} not found.");

        await repository.DeleteAsync(canvass, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new DeleteCanvassResponse(canvass.Id);
    }
}
