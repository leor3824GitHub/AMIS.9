using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Post.v1;

public sealed record PostSuppliesAndMaterialsIssuanceReportCommand(Guid Id) : IRequest<PostSuppliesAndMaterialsIssuanceReportResponse>;
