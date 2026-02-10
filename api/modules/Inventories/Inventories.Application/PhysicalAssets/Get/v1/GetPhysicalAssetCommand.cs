using MediatR;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

public sealed record GetPhysicalAssetCommand(Guid Id) : IRequest<PhysicalAssetResponse>;

public sealed record PhysicalAssetResponse(
    Guid Id,
    string PropertyCode,
    Guid ProductId,
    string? ProductName,
    decimal AcquisitionCost,
    DateTime AcquisitionDate,
    PropertyClassification Classification,
    int Quantity,
    string? SerialNumber,
    string? ModelNumber,
    string? Location,
    decimal AccumulatedDepreciation,
    decimal BookValue,
    string RCAAccountCode,
    Guid? CurrentCustodianId,
    string? CurrentCustodianName,
    Guid? ParentAssetId,
    string? ParentAssetProductName,
    string? ParentAssetPropertyCode);

