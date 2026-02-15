using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.List.v1;

public sealed record ListPARsQuery : IRequest<ListPARsResponse>;

public sealed record PARDto
{
    public Guid Id { get; init; }
    public string PARNumber { get; init; } = default!;
    public Guid EmployeeId { get; init; }
    public DateTime IssuanceDate { get; init; }
    public string Status { get; init; } = default!;
    public int LineItemsCount { get; init; }
    public decimal TotalAcquisitionCost { get; init; }
    public DateTime? ReturnDate { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record ListPARsResponse(IReadOnlyList<PARDto> PARs);
