using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Get.v1;

public sealed record GetSuppliesAndMaterialsIssuanceReportByIdQuery(Guid Id)
    : IRequest<GetSuppliesAndMaterialsIssuanceReportByIdResponse>;


