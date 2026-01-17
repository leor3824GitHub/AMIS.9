using AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;
using MediatR;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Get.v1;

public record GetSuppliesAndMaterialsIssuanceReportByIdQuery(Guid Id)
    : IRequest<GetSuppliesAndMaterialsIssuanceReportByIdResponse>;

public record GetSuppliesAndMaterialsIssuanceReportByIdResponse(
    SuppliesAndMaterialsIssuanceReportDto Report
);
