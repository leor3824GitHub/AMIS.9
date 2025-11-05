using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;

public sealed class MarkAcceptedHandler : IRequestHandler<MarkAcceptedCommand, MarkAcceptedResponse>
{
    private readonly IRepository<InspectionRequest> _repository;

    public MarkAcceptedHandler([FromKeyedServices("catalog:inspectionRequests")] IRepository<InspectionRequest> repository)
    {
        _repository = repository;
    }

    public async Task<MarkAcceptedResponse> Handle(MarkAcceptedCommand request, CancellationToken cancellationToken)
    {
        var inspectionRequest = await _repository.GetByIdAsync(request.InspectionRequestId, cancellationToken)
            ?? throw new InvalidOperationException($"InspectionRequest with ID {request.InspectionRequestId} not found.");

        inspectionRequest.MarkAccepted();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new MarkAcceptedResponse(
            inspectionRequest.Id,
            inspectionRequest.Status,
            "Inspection request marked as accepted successfully."
        );
    }
}
