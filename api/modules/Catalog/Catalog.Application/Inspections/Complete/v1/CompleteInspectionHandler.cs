using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;

public sealed class CompleteInspectionHandler : IRequestHandler<CompleteInspectionCommand, CompleteInspectionResponse>
{
    private readonly IRepository<Inspection> _repository;

    public CompleteInspectionHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<CompleteInspectionResponse> Handle(CompleteInspectionCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Inspection with ID {request.InspectionId} not found.");

        inspection.Complete();
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new CompleteInspectionResponse(
            inspection.Id,
            inspection.Status,
            "Inspection completed successfully."
        );
    }
}
