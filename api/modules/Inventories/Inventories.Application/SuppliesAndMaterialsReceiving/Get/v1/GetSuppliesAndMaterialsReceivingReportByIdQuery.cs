using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Get.v1;

public sealed record GetSuppliesAndMaterialsReceivingReportByIdQuery(Guid Id)
    : IRequest<GetSuppliesAndMaterialsReceivingReportByIdResponse>;


