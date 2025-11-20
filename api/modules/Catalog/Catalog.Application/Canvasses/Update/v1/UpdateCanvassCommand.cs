using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Update.v1;

public sealed record UpdateCanvassCommand(
    Guid Id,
    string ItemDescription,
    int Quantity,
    string Unit,
    decimal QuotedPrice,
    string? Remarks,
    DateTime ResponseDate
) : IRequest<UpdateCanvassResponse>;
