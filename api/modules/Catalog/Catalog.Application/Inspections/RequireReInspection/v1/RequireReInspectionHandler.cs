using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.RequireReInspection.v1;

public sealed class RequireReInspectionHandler : IRequestHandler<RequireReInspectionCommand, RequireReInspectionResponse>
{
    private readonly IRepository<Inspection> _repository;

    public RequireReInspectionHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<RequireReInspectionResponse> Handle(RequireReInspectionCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Inspection with ID {request.InspectionId} not found.");

        inspection.RequireReInspection(request.Reason);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new RequireReInspectionResponse(
            inspection.Id,
            inspection.Status,
            "Re-inspection required successfully.",
            request.Reason
        );
    }
}
