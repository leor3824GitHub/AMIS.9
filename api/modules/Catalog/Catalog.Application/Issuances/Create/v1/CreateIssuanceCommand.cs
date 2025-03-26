using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Create.v1;
public sealed record CreateIssuanceCommand(
    Guid EmployeeId,
    [property: DefaultValue(0)] decimal TotalAmount,
    [property: DefaultValue("Pending")] string Status = "Pending") : IRequest<CreateIssuanceResponse>;
