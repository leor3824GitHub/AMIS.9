using MediatR;
using System.ComponentModel;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Update.v1;

public sealed record UpdateAcceptanceCommand(
    Guid Id,
    DateTime AcceptanceDate,
    Guid SupplyOfficerId,
    Guid PurchaseId,
    string? Remarks
) : IRequest<UpdateAcceptanceResponse>;

