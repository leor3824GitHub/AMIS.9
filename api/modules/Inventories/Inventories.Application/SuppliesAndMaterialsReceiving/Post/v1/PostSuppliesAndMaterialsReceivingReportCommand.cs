using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;

public sealed record PostSuppliesAndMaterialsReceivingReportCommand(Guid Id) : IRequest<PostSuppliesAndMaterialsReceivingReportResponse>;
