using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ConditionallyApprove.v1;

public sealed class ConditionallyApproveHandler : IRequestHandler<ConditionallyApproveCommand, ConditionallyApproveResponse>
{
    private readonly IRepository<Inspection> _repository;

    public ConditionallyApproveHandler([FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    {
        _repository = repository;
    }

    public async Task<ConditionallyApproveResponse> Handle(ConditionallyApproveCommand request, CancellationToken cancellationToken)
    {
        var inspection = await _repository.GetByIdAsync(request.InspectionId, cancellationToken)
            ?? throw new InspectionNotFoundException(request.InspectionId);

        inspection.ConditionallyApprove(request.Conditions);
        
        await _repository.SaveChangesAsync(cancellationToken);

        return new ConditionallyApproveResponse(
            inspection.Id,
            inspection.Status,
            "Inspection conditionally approved successfully.",
            request.Conditions
        );
    }
}
