using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.Cancel.v1;

public sealed record CancelInspectionCommand(Guid Id, string? Reason) : IRequest<CancelInspectionResponse>;
public sealed record CancelInspectionResponse(Guid Id);

