using AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;
using AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Get.v1;
using AMIS.Modules.Catalog.Application.MaterialsIssuance.Interfaces;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Get.v1;

public class GetSuppliesAndMaterialsIssuanceReportByIdHandler
    : IRequestHandler<GetSuppliesAndMaterialsIssuanceReportByIdQuery, GetSuppliesAndMaterialsIssuanceReportByIdResponse>
{
    private readonly ISuppliesAndMaterialsIssuanceReportReadRepository _readRepository;

    public GetSuppliesAndMaterialsIssuanceReportByIdHandler(
        ISuppliesAndMaterialsIssuanceReportReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<GetSuppliesAndMaterialsIssuanceReportByIdResponse> Handle(
        GetSuppliesAndMaterialsIssuanceReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var report = await _readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (report == null)
            throw new KeyNotFoundException($"SMIR with ID {request.Id} not found");

        var dto = MapToDto(report);

        return new GetSuppliesAndMaterialsIssuanceReportByIdResponse(dto);
    }

    private static SuppliesAndMaterialsIssuanceReportDto MapToDto(
        SuppliesAndMaterialsIssuanceReport report)
    {
        return new SuppliesAndMaterialsIssuanceReportDto
        {
            Id = report.Id,
            SmirNumber = report.SmirNumber,
            TransactionDate = report.TransactionDate,
            Recipient = new RecipientInfoDto
            {
                Name = report.Recipient.Name,
                Address = report.Recipient.Address,
                ContactNumber = report.Recipient.ContactNumber
            },
            IssuanceReason = report.IssuanceReason.Value,
            LineItems = report.LineItems.Select(li => new IssuanceLineItemDto
            {
                Name = li.Name,
                Description = li.Description,
                AcquisitionDate = li.AcquisitionDate,
                Quantity = li.Quantity,
                Unit = li.Unit,
                UnitCost = li.UnitCost,
                Amount = li.Amount
            }).ToList().AsReadOnly(),
            Authorization = new IssuanceAuthorizationDto
            {
                IssuingOfficerName = report.Authorization.IssuingOfficerName,
                IssuingOfficerSignature = report.Authorization.IssuingOfficerSignature,
                IssuingDate = report.Authorization.IssuingDate,
                ApprovingOfficerName = report.Authorization.ApprovingOfficerName,
                ApprovingOfficerSignature = report.Authorization.ApprovingOfficerSignature,
                ApprovingDate = report.Authorization.ApprovingDate,
                RecipientName = report.Authorization.RecipientName,
                RecipientSignature = report.Authorization.RecipientSignature,
                ReceiptDate = report.Authorization.ReceiptDate,
                DriverName = report.Authorization.DriverName,
                DriverSignature = report.Authorization.DriverSignature,
                BillOfLadingNumber = report.Authorization.BillOfLadingNumber
            },
            TotalAmount = report.GetTotalAmount(),
            DistributionStatus = report.GetDistributionStatus(),
            CreatedAt = report.CreatedAt,
            CreatedBy = report.CreatedBy,
            UpdatedAt = report.UpdatedAt,
            UpdatedBy = report.UpdatedBy,
            Notes = report.Notes
        };
    }
}
