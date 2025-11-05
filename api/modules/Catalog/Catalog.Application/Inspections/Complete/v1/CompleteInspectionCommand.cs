using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;

public sealed record CompleteInspectionCommand(Guid InspectionId) : IRequest<CompleteInspectionResponse>;
