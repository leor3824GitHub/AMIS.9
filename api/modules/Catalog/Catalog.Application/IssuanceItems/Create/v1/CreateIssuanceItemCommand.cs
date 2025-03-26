using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Create.v1;
public sealed record CreateIssuanceItemCommand(
    Guid IssuanceId,
    Guid ProductId,
    [property: DefaultValue(1)] int Qty,
    [property: DefaultValue(0)] decimal UnitPrice,
    [property: DefaultValue("Pending")] string Status = "Pending") : IRequest<CreateIssuanceItemResponse>;
