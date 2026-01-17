using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inspections.Approve.v1;

public sealed record ApproveInspectionCommand(Guid Id) : IRequest<ApproveInspectionResponse>;

public sealed record ApproveInspectionResponse(Guid? Id);

