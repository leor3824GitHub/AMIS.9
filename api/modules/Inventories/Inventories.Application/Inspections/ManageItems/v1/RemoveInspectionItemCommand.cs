using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.ManageItems.v1;

public sealed record RemoveInspectionItemCommand(Guid InspectionId, Guid ItemId) : IRequest;

