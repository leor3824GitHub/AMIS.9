using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.Complete.v1;

public sealed record CompleteInspectionCommand(Guid Id) : IRequest<CompleteInspectionResponse>;
public sealed record CompleteInspectionResponse(Guid Id);

