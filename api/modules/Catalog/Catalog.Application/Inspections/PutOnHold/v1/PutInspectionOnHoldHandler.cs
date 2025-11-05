using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.PutOnHold.v1;

public sealed class PutInspectionOnHoldHandler : IRequestHandler<PutInspectionOnHoldCommand, PutInspectionOnHoldResponse>
{
    private readonly IRepository<Inspection> _repository;

    public PutInspectionOnHoldHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<PutInspectionOnHoldResponse> Handle(PutInspectionOnHoldCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Inspection with ID {request.InspectionId} not found.");

        inspection.PutOnHold(request.Reason);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new PutInspectionOnHoldResponse(
            inspection.Id,
            inspection.Status,
            "Inspection put on hold successfully.",
            request.Reason
        );
    }
}
