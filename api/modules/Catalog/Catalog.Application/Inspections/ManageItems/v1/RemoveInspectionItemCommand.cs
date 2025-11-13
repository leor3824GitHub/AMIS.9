using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed record RemoveInspectionItemCommand(Guid InspectionId, Guid ItemId) : IRequest;
