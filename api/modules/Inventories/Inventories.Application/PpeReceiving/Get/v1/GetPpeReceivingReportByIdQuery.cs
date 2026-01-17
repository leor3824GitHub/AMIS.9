using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed record GetPpeReceivingReportByIdQuery(Guid Id) : IRequest<GetPpeReceivingReportByIdResponse>;

