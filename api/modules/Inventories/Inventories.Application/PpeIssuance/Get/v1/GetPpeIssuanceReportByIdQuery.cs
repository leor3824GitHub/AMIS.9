using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed record GetPpeIssuanceReportByIdQuery(Guid Id) : IRequest<GetPpeIssuanceReportByIdResponse>;

