using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;

public sealed class MarkCompletedHandler : IRequestHandler<MarkCompletedCommand, MarkCompletedResponse>
{
    private readonly IRepository<InspectionRequest> _repository;

    public MarkCompletedHandler([FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    {
        _repository = repository;
    }

    public async Task<MarkCompletedResponse> Handle(MarkCompletedCommand request, CancellationToken cancellationToken)
    {
        var inspectionRequest = await _repository.GetByIdAsync(request.InspectionRequestId, cancellationToken)
            ?? throw new InvalidOperationException($"InspectionRequest with ID {request.InspectionRequestId} not found.");

        inspectionRequest.MarkCompleted();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkCompletedResponse(
            inspectionRequest.Id,
            inspectionRequest.Status,
            "Inspection request marked as completed successfully."
        );
    }
}
