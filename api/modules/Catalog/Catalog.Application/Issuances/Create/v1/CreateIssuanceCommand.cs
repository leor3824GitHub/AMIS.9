using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Create.v1;
public sealed record CreateIssuanceCommand(
    Guid ProductId,
    Guid EmployeeId,
    [property: DefaultValue(1)] decimal Qty,
    [property: DefaultValue("pc")] string Unit,
    [property: DefaultValue(0)] decimal UnitPrice) : IRequest<CreateIssuanceResponse>;
