using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.AssignCustodian.v1;

public sealed class AssignCustodianHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<AssignCustodianCommand, AssignCustodianResponse>
{
    public async Task<AssignCustodianResponse> Handle(AssignCustodianCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.AssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset with ID {request.AssetId} not found.");

        // DEPRECATED: Direct custodian assignment no longer supported.
        // CurrentCustodianId is now computed from CurrentAssignment.EmployeeId
        // Use Issue(), Transfer(), or Return() methods to manage asset custody through proper assignment history.
        throw new NotImplementedException(
            "Direct custodian assignment is no longer supported. " +
            "Use Issue(), Transfer(), or Return() methods which create proper assignment history records.");
    }
}

