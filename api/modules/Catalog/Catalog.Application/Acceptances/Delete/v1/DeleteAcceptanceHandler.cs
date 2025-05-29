using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Delete.v1
{
    public sealed class DeleteAcceptanceHandler(
        ILogger<DeleteAcceptanceHandler> logger,
        [FromKeyedServices("catalog:inspections")] IRepository<Acceptance> repository)
        : IRequestHandler<DeleteAcceptanceCommand, DeleteAcceptanceResponse>
    {
        public async Task<DeleteAcceptanceResponse> Handle(DeleteAcceptanceCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
            _ = inspection ?? throw new AcceptanceNotFoundException(request.Id);

            await repository.DeleteAsync(inspection, cancellationToken);
            logger.LogInformation("Acceptance deleted: {AcceptanceId}", request.Id);

            return new DeleteAcceptanceResponse(request.Id);
        }
    }
}
