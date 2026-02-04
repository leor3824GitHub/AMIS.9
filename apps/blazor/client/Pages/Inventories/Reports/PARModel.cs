namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

/// <summary>
/// UI model for Property Acknowledgement Receipt (PAR) form.
/// Bridges between API DTOs and UI form controls.
/// </summary>
public class PARModel
{
    public string? PARNumber { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime IssuanceDate { get; set; } = DateTime.Today;
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public string? Notes { get; set; }
    public string? IssuedByName { get; set; }
    public DateTime? IssuedByDate { get; set; }
    public string? ReceivedByName { get; set; }
    public DateTime? ReceivedByDate { get; set; }
    public string? ApprovedByName { get; set; }
    public DateTime? ApprovedByDate { get; set; }
    public List<PARLineItemModel> LineItems { get; set; } = new();
}

public class PARLineItemModel
{
    public string? PropertyCode { get; set; }
    public string? Description { get; set; }
    public DateTime DateAcquired { get; set; }
    public decimal AcquisitionCost { get; set; }
    public string? Condition { get; set; }
    public string? Remarks { get; set; }
}
