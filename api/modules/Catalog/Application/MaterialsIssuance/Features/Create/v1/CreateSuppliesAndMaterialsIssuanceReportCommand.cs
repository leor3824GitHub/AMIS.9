using AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;
using MediatR;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Create.v1;

public record CreateSuppliesAndMaterialsIssuanceReportCommand(
    string SmirNumber,
    DateTime TransactionDate,
    RecipientInfoDto Recipient,
    string IssuanceReason,
    List<IssuanceLineItemDto> LineItems,
    IssuanceAuthorizationDto Authorization,
    string? Notes
) : IRequest<CreateSuppliesAndMaterialsIssuanceReportResponse>;

public record CreateSuppliesAndMaterialsIssuanceReportResponse(
    Guid Id,
    string SmirNumber,
    decimal TotalAmount
);
