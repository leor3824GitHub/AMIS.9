using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Update.v1;
public sealed record UpdateIssuanceCommand(
    Guid Id,
    Guid ProductId,
    Guid EmployeeId,
    decimal Qty,
    string Unit,
    decimal UnitPrice) : IRequest<UpdateIssuanceResponse>;
