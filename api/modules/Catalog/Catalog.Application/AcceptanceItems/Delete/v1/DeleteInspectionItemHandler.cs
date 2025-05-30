using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Delete.v1
{
    public sealed class DeleteAcceptanceItemHandler(
        ILogger<DeleteAcceptanceItemHandler> logger,
        [FromKeyedServices("catalog:inspectionItems")] IRepository<AcceptanceItem> repository)
        : IRequestHandler<DeleteAcceptanceItemCommand, DeleteAcceptanceItemResponse>
    {
        public async Task<DeleteAcceptanceItemResponse> Handle(DeleteAcceptanceItemCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
            _ = inspection ?? throw new AcceptanceItemNotFoundException(request.Id);

            await repository.DeleteAsync(inspection, cancellationToken);
            logger.LogInformation("Acceptance item deleted: {AcceptanceId}", request.Id);

            return new DeleteAcceptanceItemResponse(request.Id);
        }
    }
}
