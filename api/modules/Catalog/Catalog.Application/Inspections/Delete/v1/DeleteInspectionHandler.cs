using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Application.Inspections.Delete.v1
{
    public sealed class DeleteInspectionHandler(
        ILogger<DeleteInspectionHandler> logger,
        [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository,
        [FromKeyedServices("catalog:acceptances")] IReadRepository<Acceptance> acceptanceReadRepository)
        : IRequestHandler<DeleteInspectionCommand, DeleteInspectionResponse>
    {
        public async Task<DeleteInspectionResponse> Handle(DeleteInspectionCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
            _ = inspection ?? throw new InspectionNotFoundException(request.Id);

            // Integrity guard: prevent deleting an inspection if it already has a related acceptance
            var hasAcceptance = await acceptanceReadRepository.AnyAsync(new AMIS.WebApi.Catalog.Application.Acceptances.Specifications.AcceptancesByInspectionIdSpec(request.Id), cancellationToken);
            if (hasAcceptance)
            {
                throw new InvalidOperationException("Cannot delete inspection because an acceptance has been recorded for it.");
            }

            await repository.DeleteAsync(inspection, cancellationToken);
            logger.LogInformation("Inspection deleted: {InspectionId}", request.Id);

            return new DeleteInspectionResponse(request.Id);
        }
    }
}
