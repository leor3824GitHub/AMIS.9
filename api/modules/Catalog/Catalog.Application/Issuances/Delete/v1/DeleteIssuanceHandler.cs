using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Issuances.Delete.v1;
public sealed class DeleteIssuanceHandler(
    ILogger<DeleteIssuanceHandler> logger,
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<DeleteIssuanceCommand>
{
    public async Task Handle(DeleteIssuanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var issuance = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = issuance ?? throw new IssuanceNotFoundException(request.Id);
        await repository.DeleteAsync(issuance, cancellationToken);
        logger.LogInformation("issuance with id : {IssuanceId} deleted", issuance.Id);
    }
}
