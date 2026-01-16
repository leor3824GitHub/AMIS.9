using MediatR;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Get.v1;

public sealed record GetPhysicalAssetCommand(Guid Id) : IRequest<PhysicalAssetResponse>;

public sealed record PhysicalAssetResponse(
    Guid Id,
    string PropertyCode,
    Guid ProductId,
    string Description,
    decimal AcquisitionCost,
    DateTime AcquisitionDate,
    int EstimatedUsefulLife,
    PropertyClassification Classification,
    int Quantity,
    string UnitOfMeasure,
    string? SerialNumber,
    string? ModelNumber,
    string? Location,
    string? PPEType,
    decimal AccumulatedDepreciation,
    decimal BookValue,
    string RCAAccountCode,
    Guid? CurrentCustodianId);
