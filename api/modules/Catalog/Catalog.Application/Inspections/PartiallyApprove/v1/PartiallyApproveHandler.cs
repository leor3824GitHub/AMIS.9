using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.PartiallyApprove.v1;

public sealed class PartiallyApproveHandler : IRequestHandler<PartiallyApproveCommand, PartiallyApproveResponse>
{
    private readonly IRepository<Inspection> _repository;

    public PartiallyApproveHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<PartiallyApproveResponse> Handle(PartiallyApproveCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InspectionNotFoundException(request.InspectionId);

        inspection.PartiallyApprove(request.PartialDetails);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new PartiallyApproveResponse(
            inspection.Id,
            inspection.Status,
            "Inspection partially approved successfully.",
            request.PartialDetails
        );
    }
}
