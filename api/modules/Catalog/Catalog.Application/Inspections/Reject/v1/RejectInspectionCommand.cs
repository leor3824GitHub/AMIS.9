using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Reject.v1;

public sealed record RejectInspectionCommand(Guid Id, string? Reason) : IRequest<RejectInspectionResponse>;
public sealed record RejectInspectionResponse(Guid Id);
