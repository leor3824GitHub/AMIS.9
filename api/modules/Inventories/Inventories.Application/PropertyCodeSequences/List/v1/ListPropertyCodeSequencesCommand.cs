using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.List.v1;

public sealed record ListPropertyCodeSequencesCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null) : IRequest<ListPropertyCodeSequencesResponse>;

public sealed record PropertyCodeSequenceDto(
    int Id,
    string ClassCode,
    string Classification,
    string CategoryCode,
    string ItemCode,
    string ItemDescription,
    string? GLAccount,
    int LastSequenceValue);

public sealed record ListPropertyCodeSequencesResponse(
    IEnumerable<PropertyCodeSequenceDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);
