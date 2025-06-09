using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Update.v1;

public sealed record UpdateAcceptanceCommand(
    Guid Id,
    DateTime AcceptanceDate,
    Guid SupplyOfficerId,
    Guid PurchaseId,
    string? Remarks
) : IRequest<UpdateAcceptanceResponse>;
