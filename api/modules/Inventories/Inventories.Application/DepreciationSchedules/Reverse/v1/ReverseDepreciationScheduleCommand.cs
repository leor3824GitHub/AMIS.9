using MediatR;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Reverse.v1;

/// <summary>
/// Command to reverse a depreciation schedule entry
/// </summary>
public record ReverseDepreciationScheduleCommand : IRequest<ReverseDepreciationScheduleResponse>
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
}

/// <summary>
/// Response for reversing a depreciation schedule entry
/// </summary>
public record ReverseDepreciationScheduleResponse
{
    public Guid Id { get; init; }
    public string Status { get; init; } = "Reversed";
    public string? Reason { get; init; }
    public string Message { get; init; } = "Depreciation schedule reversed";
}

