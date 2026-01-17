using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.IssuanceItems.Update.v1;
public sealed record UpdateIssuanceItemCommand(
    Guid Id,
    Guid IssuanceId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending") : IRequest<UpdateIssuanceItemResponse>;

