using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.GetById.v1;

public sealed record GetPropertyCodeSequenceCommand(int Id) : IRequest<GetPropertyCodeSequenceResponse>;

public sealed record GetPropertyCodeSequenceResponse(
    int Id,
    string ClassCode,
    string Classification,
    string CategoryCode,
    string ItemCode,
    string ItemDescription,
    string? GLAccount,
    int LastSequenceValue);
