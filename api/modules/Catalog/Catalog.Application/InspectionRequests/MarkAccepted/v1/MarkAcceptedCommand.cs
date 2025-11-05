using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;

public sealed record MarkAcceptedCommand(Guid InspectionRequestId) : IRequest<MarkAcceptedResponse>;
