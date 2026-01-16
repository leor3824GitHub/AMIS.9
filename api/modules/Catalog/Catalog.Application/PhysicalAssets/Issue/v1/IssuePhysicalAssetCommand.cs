using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.Issue.v1;

/// <summary>
/// Command to issue a physical asset to an employee
/// </summary>
public record IssuePhysicalAssetCommand : IRequest<IssuePhysicalAssetResponse>
{
    public Guid Id { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string DocumentNumber { get; init; } = string.Empty;
    public int? QuantityIssued { get; init; }
}

/// <summary>
/// Response for issuing a physical asset
/// </summary>
public record IssuePhysicalAssetResponse
{
    public Guid AssetId { get; init; }
    public Guid AssignmentHistoryId { get; init; }
    public string EmployeeName { get; init; } = string.Empty;
    public string DocumentNumber { get; init; } = string.Empty;
    public DateTime IssuedDate { get; init; }
    public string Message { get; init; } = "Asset issued successfully";
}
