using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Create.v1;
public sealed record CreateIssuanceCommand(
    Guid EmployeeId,
    DateTime IssuanceDate,
    [property: DefaultValue(0)] decimal TotalAmount) : IRequest<CreateIssuanceResponse>;
