using MediatR;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Create.v1;

public sealed record CreatePhysicalAssetCommand(
    string PropertyCode,
    Guid ProductId,
    string Description,
    decimal AcquisitionCost,
    DateTime AcquisitionDate,
    int EstimatedUsefulLife,
    PropertyClassification Classification,
    int Quantity = 1,
    string UnitOfMeasure = "piece",
    string? SerialNumber = null,
    string? ModelNumber = null,
    string? Location = null,
    string? PPEType = null) : IRequest<CreatePhysicalAssetResponse>;

public sealed record CreatePhysicalAssetResponse(
    Guid Id,
    string PropertyCode,
    string Description,
    decimal AcquisitionCost,
    PropertyClassification Classification);
