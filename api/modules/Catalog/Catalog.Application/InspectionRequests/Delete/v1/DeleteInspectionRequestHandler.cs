using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Delete.v1
{
    public sealed class DeleteInspectionRequestHandler(
        ILogger<DeleteInspectionRequestHandler> logger,
        [FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
        : IRequestHandler<DeleteInspectionRequestCommand, DeleteInspectionRequestResponse>
    {
        public async Task<DeleteInspectionRequestResponse> Handle(DeleteInspectionRequestCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inspectionRequest = await repository.GetByIdAsync(request.Id, cancellationToken);
            _ = inspectionRequest ?? throw new InspectionRequestNotFoundException(request.Id);

            await repository.DeleteAsync(inspectionRequest, cancellationToken);
            logger.LogInformation("InspectionRequest deleted: {InspectionRequestId}", request.Id);

            return new DeleteInspectionRequestResponse(request.Id);
        }
    }
}
