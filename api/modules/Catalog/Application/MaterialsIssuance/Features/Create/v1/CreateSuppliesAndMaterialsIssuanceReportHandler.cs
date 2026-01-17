using AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;
using AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Create.v1;
using AMIS.Modules.Catalog.Application.MaterialsIssuance.Interfaces;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Create.v1;

public class CreateSuppliesAndMaterialsIssuanceReportHandler
    : IRequestHandler<CreateSuppliesAndMaterialsIssuanceReportCommand, CreateSuppliesAndMaterialsIssuanceReportResponse>
{
    private readonly ISuppliesAndMaterialsIssuanceReportRepository _repository;

    public CreateSuppliesAndMaterialsIssuanceReportHandler(
        ISuppliesAndMaterialsIssuanceReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateSuppliesAndMaterialsIssuanceReportResponse> Handle(
        CreateSuppliesAndMaterialsIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        // Map DTOs to domain objects
        var recipient = new RecipientInfo(
            request.Recipient.Name,
            request.Recipient.Address,
            request.Recipient.ContactNumber);

        var issuanceReason = IssuanceReason.FromString(request.IssuanceReason);

        var authorization = new IssuanceAuthorization(
            request.Authorization.IssuingOfficerName,
            request.Authorization.IssuingOfficerSignature,
            request.Authorization.IssuingDate,
            request.Authorization.ApprovingOfficerName,
            request.Authorization.ApprovingOfficerSignature,
            request.Authorization.ApprovingDate,
            request.Authorization.RecipientName,
            request.Authorization.RecipientSignature,
            request.Authorization.ReceiptDate,
            request.Authorization.DriverName,
            request.Authorization.DriverSignature,
            request.Authorization.BillOfLadingNumber);

        // Create the aggregate root
        var report = new SuppliesAndMaterialsIssuanceReport(
            request.SmirNumber,
            request.TransactionDate,
            recipient,
            issuanceReason,
            authorization,
            "system", // TODO: Get from current user context
            request.Notes);

        // Add line items
        var lineItems = request.LineItems.Select(dto => new IssuanceLineItem(
            dto.Name,
            dto.Description,
            dto.AcquisitionDate,
            dto.Quantity,
            dto.Unit,
            dto.UnitCost)).ToList();

        report.AddLineItems(lineItems);

        // Persist
        await _repository.AddAsync(report, cancellationToken);

        return new CreateSuppliesAndMaterialsIssuanceReportResponse(
            report.Id,
            report.SmirNumber,
            report.GetTotalAmount());
    }
}
