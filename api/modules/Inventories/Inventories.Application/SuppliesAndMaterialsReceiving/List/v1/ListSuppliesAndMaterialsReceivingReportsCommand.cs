using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.List.v1;

public sealed record ListSuppliesAndMaterialsReceivingReportsCommand() : IRequest<ListSuppliesAndMaterialsReceivingReportsResponse>;

public sealed record SuppliesAndMaterialsReceivingReportDto(
    Guid Id,
    string SmrrNumber,
    string SourceName,
    string TransactionType,
    DateTime ReceivingDate,
    int LineItemsCount,
    decimal TotalAmount,
    int Status,
    DateTimeOffset CreatedAt);

public sealed record ListSuppliesAndMaterialsReceivingReportsResponse(
    IReadOnlyList<SuppliesAndMaterialsReceivingReportDto> Reports);
