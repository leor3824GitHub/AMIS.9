using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.PropertyCodes;

public sealed record CoaPropertyCodeRequest(
    DateTime AcquisitionDate,
    PropertyClassification Classification,
    string? OfficeCode,
    string? ClassCode,
    string? CategoryCode,
    string? ItemCode,
    string? SequenceSuffix = null);
