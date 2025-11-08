using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromHold.v1;

public sealed class ReleaseInspectionFromHoldHandler : IRequestHandler<ReleaseInspectionFromHoldCommand, ReleaseInspectionFromHoldResponse>
{
    private readonly IRepository<Inspection> _repository;

    public ReleaseInspectionFromHoldHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<ReleaseInspectionFromHoldResponse> Handle(ReleaseInspectionFromHoldCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InspectionNotFoundException(request.InspectionId);

        inspection.ReleaseFromHold();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new ReleaseInspectionFromHoldResponse(
            inspection.Id,
            inspection.Status,
            "Inspection released from hold successfully."
        );
    }
}
