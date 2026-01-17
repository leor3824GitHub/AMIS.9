using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Create.v1;

public sealed record CreateAssetRequisitionCommand(
    Guid EmployeeId,
    Guid IssuanceId) : IRequest<CreateAssetRequisitionResponse>;

public sealed record CreateAssetRequisitionResponse(Guid Id);

