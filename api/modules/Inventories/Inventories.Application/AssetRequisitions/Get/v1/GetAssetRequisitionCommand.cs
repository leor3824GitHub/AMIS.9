using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Get.v1;

public sealed record GetAssetRequisitionCommand(Guid Id) : IRequest<GetAssetRequisitionResponse>;

public sealed record GetAssetRequisitionResponse(
    Guid Id,
    Guid EmployeeId,
    Guid IssuanceId,
    DateTime RequisitionDate,
    string Status,
    DateTime? ExpirationDate,
    bool IsExpired);

