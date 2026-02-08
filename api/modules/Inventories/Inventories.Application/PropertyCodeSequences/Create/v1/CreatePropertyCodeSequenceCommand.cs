using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Create.v1;

public sealed record CreatePropertyCodeSequenceCommand(
    string Classification,
    string Category) : IRequest<CreatePropertyCodeSequenceResponse>;

public sealed record CreatePropertyCodeSequenceResponse(
    int Id,
    string ClassCode,
    string Classification,
    string CategoryCode,
    string ItemCode,
    string ItemDescription,
    string? GLAccount,
    int LastSequenceValue);
