using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodes.Generate.v1;

public sealed record GeneratePropertyCodeCommand(
    DateTime AcquisitionDate,
    PropertyClassification Classification,
    string? OfficeCode = null,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null,
    string? SequenceSuffix = null) : IRequest<GeneratePropertyCodeResponse>;

public sealed record GeneratePropertyCodeResponse(
    string Classification,
    string Category,
    int Sequence);
