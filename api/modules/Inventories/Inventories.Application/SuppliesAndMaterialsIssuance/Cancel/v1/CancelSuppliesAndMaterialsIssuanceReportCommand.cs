using MediatR;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Cancel.v1;

public sealed record CancelSuppliesAndMaterialsIssuanceReportCommand(Guid Id) : IRequest<CancelSuppliesAndMaterialsIssuanceReportResponse>;
