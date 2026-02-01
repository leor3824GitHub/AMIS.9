using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Get.v1;

public sealed record GetPARByIdQuery(Guid Id) : IRequest<GetPARByIdResponse>;

public sealed record PARLineItemDto
{
    public string PropertyCode { get; init; } = default!;
    public string Description { get; init; } = default!;
    public DateTime DateAcquired { get; init; }
    public decimal AcquisitionCost { get; init; }
    public string? Condition { get; init; }
    public string? Remarks { get; init; }
}

public sealed record GetPARByIdResponse
{
    public Guid Id { get; init; }
    public string PARNumber { get; init; } = default!;
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = default!;
    public string Department { get; init; } = default!;
    public string? Position { get; init; }
    public DateTime IssuanceDate { get; init; }
    public string? IssuancePurpose { get; init; }
    public string? IssuanceLocation { get; init; }
    public string Status { get; init; } = default!;
    public IReadOnlyList<PARLineItemDto> LineItems { get; init; } = [];
    public DateTime? ReturnDate { get; init; }
    public string? ReturnRemarks { get; init; }
    public Guid? ReceivedByEmployeeId { get; init; }
    public string? ReceivedByEmployeeName { get; init; }
    public string? IssuedByName { get; init; }
    public DateTime? IssuedByDate { get; init; }
    public string? ReceivedByName { get; init; }
    public DateTime? ReceivedByDate { get; init; }
    public string? ApprovedByName { get; init; }
    public DateTime? ApprovedByDate { get; init; }
    public string? Notes { get; init; }
    public decimal TotalAcquisitionCost { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime LastModifiedAt { get; init; }
}
