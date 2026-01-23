using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.List.v1;

public sealed record ListSuppliesAndMaterialsIssuanceReportsCommand() : IRequest<ListSuppliesAndMaterialsIssuanceReportsResponse>;

public sealed record SuppliesAndMaterialsIssuanceReportDto(
    Guid Id,
    string SmirNumber,
    string IssuedToName,
    string TransactionType,
    DateTime IssuanceDate,
    int LineItemsCount,
    decimal TotalAmount,
    int Status,
    DateTimeOffset CreatedAt);

public sealed record ListSuppliesAndMaterialsIssuanceReportsResponse(
    IReadOnlyList<SuppliesAndMaterialsIssuanceReportDto> Reports);
