using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
public sealed record UpdateIssuanceItemCommand(
    Guid Id,
    Guid IssuanceId,
    Guid ProductId,
    int Qty,
    decimal UnitPrice,
    string? Status = "Pending") : IRequest<UpdateIssuanceItemResponse>;
