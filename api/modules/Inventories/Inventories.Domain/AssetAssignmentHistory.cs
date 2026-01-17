using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Tracks complete history of asset assignments (ICS and PAR)
/// Unified approach for both Semi-Expendable (ICS) and PPE (PAR) tracking
/// Critical for COA audit trail and accountability tracking
/// Part of PhysicalAsset aggregate - not a standalone aggregate root
/// </summary>
public class AssetAssignmentHistory : AuditableEntity
{
    public Guid AssetId { get; private set; }
    public string AssetNumber { get; private set; } = default!;
    public Guid EmployeeId { get; private set; }
    public string EmployeeName { get; private set; } = default!; // Denormalized for reporting
    public string DocumentNumber { get; private set; } = default!; // ICS or PAR number
    public DocumentType DocumentType { get; private set; }
    public DateTime AssignmentDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public string AssignmentType { get; private set; } = default!; // Initial, Transfer, Return
    public int Quantity { get; private set; } // For semi-expendable quantity tracking
    public PropertyClassification AssetClassification { get; private set; }
    public string? Reason { get; private set; } // For transfers/returns
    public string? Remarks { get; private set; }
    public Guid? TransferredToEmployeeId { get; private set; } // If transferred
    public string? TransferredToDocumentNumber { get; private set; } // New ICS/PAR after transfer
    public string Status { get; private set; } = "Active"; // Active, Returned, Transferred
    public string? Condition { get; private set; } // Condition at return
    public Guid? AcceptedBy { get; private set; } // Who accepted the return
    public DateTime? AcceptanceDate { get; private set; }

    // Navigation properties
    public virtual PhysicalAsset Asset { get; private set; } = default!;
    public virtual Employee Employee { get; private set; } = default!;
    public virtual Employee? TransferredToEmployee { get; private set; }
    public virtual Employee? AcceptedByEmployee { get; private set; }

    private AssetAssignmentHistory() { }

    private AssetAssignmentHistory(
        Guid id,
        Guid assetId,
        string assetNumber,
        Guid employeeId,
        string employeeName,
        string documentNumber,
        DocumentType documentType,
        DateTime assignmentDate,
        int quantity,
        PropertyClassification assetClassification,
        string assignmentType,
        string? reason = null)
    {
        Id = id;
        AssetId = assetId;
        AssetNumber = assetNumber;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        DocumentNumber = documentNumber;
        DocumentType = documentType;
        AssignmentDate = assignmentDate;
        Quantity = quantity;
        AssetClassification = assetClassification;
        AssignmentType = assignmentType;
        Reason = reason;
        Status = "Active";
    }

    /// <summary>
    /// Record initial assignment (ICS or PAR issuance)
    /// </summary>
    public static AssetAssignmentHistory CreateInitialAssignment(
        Guid assetId,
        string assetNumber,
        Guid employeeId,
        string employeeName,
        string documentNumber,
        DocumentType documentType,
        DateTime assignmentDate,
        int quantity,
        PropertyClassification assetClassification)
    {
        return new AssetAssignmentHistory(
            Guid.NewGuid(),
            assetId,
            assetNumber,
            employeeId,
            employeeName,
            documentNumber,
            documentType,
            assignmentDate,
            quantity,
            assetClassification,
            "Initial");
    }

    /// <summary>
    /// Record asset transfer to another employee
    /// </summary>
    public static AssetAssignmentHistory CreateTransferAssignment(
        Guid assetId,
        string assetNumber,
        Guid fromEmployeeId,
        string fromEmployeeName,
        string originalDocumentNumber,
        DocumentType documentType,
        Guid toEmployeeId,
        string newDocumentNumber,
        DateTime transferDate,
        int quantity,
        PropertyClassification assetClassification,
        string reason)
    {
        var history = new AssetAssignmentHistory(
            Guid.NewGuid(),
            assetId,
            assetNumber,
            fromEmployeeId,
            fromEmployeeName,
            originalDocumentNumber,
            documentType,
            transferDate,
            quantity,
            assetClassification,
            "Transfer",
            reason);

        history.TransferredToEmployeeId = toEmployeeId;
        history.TransferredToDocumentNumber = newDocumentNumber;
        history.Status = "Transferred";
        history.ReturnDate = transferDate;

        return history;
    }

    /// <summary>
    /// Mark assignment as returned
    /// </summary>
    public void MarkAsReturned(
        DateTime returnDate,
        string reason,
        string condition,
        Guid acceptedBy)
    {
        if (Status == "Returned")
            throw new InvalidOperationException("Assignment is already marked as returned.");

        ReturnDate = returnDate;
        Reason = string.IsNullOrWhiteSpace(Reason) ? reason : $"{Reason}; {reason}";
        Condition = condition;
        AcceptedBy = acceptedBy;
        AcceptanceDate = returnDate;
        Status = "Returned";
    }

    /// <summary>
    /// Add remarks or notes
    /// </summary>
    public void AddRemarks(string remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
            throw new ArgumentException("Remarks cannot be empty.");

        Remarks = string.IsNullOrWhiteSpace(Remarks)
            ? remarks
            : $"{Remarks}\n{DateTime.UtcNow:yyyy-MM-dd}: {remarks}";
    }

    /// <summary>
    /// Calculate duration of assignment in days
    /// </summary>
    public int GetAssignmentDurationDays()
    {
        var endDate = ReturnDate ?? DateTime.UtcNow;
        return (endDate - AssignmentDate).Days;
    }

    /// <summary>
    /// Check if assignment is currently active
    /// </summary>
    public bool IsActive() => Status == "Active";
}

