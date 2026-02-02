namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public class PARModel
{
    public Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Position { get; set; }
    public DateTime? IssuanceDate { get; set; }
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public string? Notes { get; set; }
    public List<PARLineItemModel> LineItems { get; set; } = [];
}

public class PARLineItemModel
{
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DateAcquired { get; set; }
    public decimal AcquisitionCost { get; set; }
    public string? Condition { get; set; }
    public string? Remarks { get; set; }
}

public class PARListItemDto
{
    public Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime IssuanceDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemsCount { get; set; }
    public decimal TotalAcquisitionCost { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PARDetailsDto
{
    public Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Position { get; set; }
    public DateTime IssuanceDate { get; set; }
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PARLineItemDetailsDto> LineItems { get; set; } = [];
    public DateTime? ReturnDate { get; set; }
    public string? ReturnRemarks { get; set; }
    public Guid? ReceivedByEmployeeId { get; set; }
    public string? ReceivedByEmployeeName { get; set; }
    public string? IssuedByName { get; set; }
    public DateTime? IssuedByDate { get; set; }
    public string? ReceivedByName { get; set; }
    public DateTime? ReceivedByDate { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedByDate { get; set; }
    public string? Notes { get; set; }
    public decimal TotalAcquisitionCost { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
}

public class PARLineItemDetailsDto
{
    public Guid Id { get; set; }
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateAcquired { get; set; }
    public decimal AcquisitionCost { get; set; }
    public string? Condition { get; set; }
    public string? Remarks { get; set; }
}

public enum PARStatus
{
    Draft = 0,
    Posted = 1,
    Returned = 2,
    Cancelled = 3
}

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Position { get; set; }
}
