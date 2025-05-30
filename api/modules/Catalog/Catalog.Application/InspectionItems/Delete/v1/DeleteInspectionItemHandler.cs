using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Delete.v1
{
    public sealed class DeleteInspectionItemHandler(
        ILogger<DeleteInspectionItemHandler> logger,
        [FromKeyedServices("catalog:acceptanceItems")] IRepository<InspectionItem> repository)
        : IRequestHandler<DeleteInspectionItemCommand, DeleteInspectionItemResponse>
    {
        public async Task<DeleteInspectionItemResponse> Handle(DeleteInspectionItemCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
            _ = inspection ?? throw new InspectionItemNotFoundException(request.Id);

            await repository.DeleteAsync(inspection, cancellationToken);
            logger.LogInformation("Inspection item deleted: {InspectionId}", request.Id);

            return new DeleteInspectionItemResponse(request.Id);
        }
    }
}
