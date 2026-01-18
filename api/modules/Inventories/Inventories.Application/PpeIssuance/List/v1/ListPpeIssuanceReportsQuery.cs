using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;

public record ListPpeIssuanceReportsQuery : IRequest<ListPpeIssuanceReportsResponse>;
