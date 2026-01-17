using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Issuances.Update.v1;
public sealed record UpdateIssuanceCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime IssuanceDate,
    decimal TotalAmount,
    bool IsClosed) : IRequest<UpdateIssuanceResponse>;

