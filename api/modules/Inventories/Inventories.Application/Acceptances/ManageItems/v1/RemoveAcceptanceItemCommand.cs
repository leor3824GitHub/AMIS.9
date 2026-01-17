using MediatR;

namespace AMIS.WebApi.Inventories.Application.Acceptances.ManageItems.v1;

public sealed record RemoveAcceptanceItemCommand(Guid AcceptanceId, Guid ItemId) : IRequest;

