using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Delete.v1;

public sealed record DeletePropertyCodeSequenceCommand(int Id) : IRequest<DeletePropertyCodeSequenceResponse>;

public sealed record DeletePropertyCodeSequenceResponse(bool Success, string Message);
