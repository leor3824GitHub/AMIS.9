using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromQuarantine.v1;

public sealed class ReleaseInspectionFromQuarantineHandler : IRequestHandler<ReleaseInspectionFromQuarantineCommand, ReleaseInspectionFromQuarantineResponse>
{
    private readonly IRepository<Inspection> _repository;

    public ReleaseInspectionFromQuarantineHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<ReleaseInspectionFromQuarantineResponse> Handle(ReleaseInspectionFromQuarantineCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InspectionNotFoundException(request.InspectionId);

        inspection.ReleaseFromQuarantine();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new ReleaseInspectionFromQuarantineResponse(
            inspection.Id,
            inspection.Status,
            "Inspection released from quarantine successfully."
        );
    }
}
