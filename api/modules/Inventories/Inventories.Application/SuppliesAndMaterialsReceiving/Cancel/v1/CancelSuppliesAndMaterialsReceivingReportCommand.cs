using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Cancel.v1;

public sealed record CancelSuppliesAndMaterialsReceivingReportCommand(Guid Id) : IRequest<CancelSuppliesAndMaterialsReceivingReportResponse>;
