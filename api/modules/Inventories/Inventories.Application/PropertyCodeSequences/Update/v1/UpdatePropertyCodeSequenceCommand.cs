using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Update.v1;

public sealed record UpdatePropertyCodeSequenceCommand(
    int Id,
    string Classification,
    string Category,
    int LastSequenceValue) : IRequest<UpdatePropertyCodeSequenceResponse>;

public sealed record UpdatePropertyCodeSequenceResponse(
    int Id,
    string ClassCode,
    string Classification,
    string CategoryCode,
    string ItemCode,
    string ItemDescription,
    string? GLAccount,
    int LastSequenceValue);
