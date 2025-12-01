using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Unified Physical Asset entity that handles both Semi-Expendable and PPE
/// Classification is dynamic based on configurable thresholds from AssetClassificationRule
/// Supports automatic reclassification when COA/DBM changes thresholds
/// </summary>
public class PhysicalAsset : AuditableEntity, IAggregateRoot
{
    // Core Identification
    public string PropertyCode { get; private set; } = default!; // PropertyCode or PropertyNumber
    public Guid ProductId { get; private set; }
    public string Description { get; private set; } = default!;

    // Acquisition Details
    public decimal AcquisitionCost { get; private set; }
    public DateTime AcquisitionDate { get; private set; }

    // Physical Attributes
    public string? SerialNumber { get; private set; }
    public string? ModelNumber { get; private set; }
    public string? Location { get; private set; }
    public string Condition { get; private set; } = "Good";

    // Quantity (for Semi-Expendable batch tracking)
    public int Quantity { get; private set; } = 1;
    public string UnitOfMeasure { get; private set; } = "piece";

    // Lifecycle
    public int EstimatedUsefulLife { get; private set; } // in months
    public DateTime? DisposalDate { get; private set; }
    public string? DisposalReason { get; private set; }

    // Classification (determined at creation/update using IAssetClassificationService)
    public PropertyClassification CurrentClassification { get; private set; }

    // Previous classification (for tracking reclassification history)
    public PropertyClassification? PreviousClassification { get; private set; }
    public DateTime? LastReclassificationDate { get; private set; }
    public string? ReclassificationReason { get; private set; }

    // PPE-Specific Fields (null if Semi-Expendable)
    public string? PPEType { get; private set; } // Machinery, ICT, etc.
    public decimal AccumulatedDepreciation { get; private set; }
    public decimal BookValue => AcquisitionCost - AccumulatedDepreciation;

    // Dynamic RCA Account (calculated based on current classification)
    public string RCAAccountCode => GetRCAAccountCode();

    // Current assignment derived from AssignmentHistory (no denormalized fields)
    // Query: AssignmentHistory.Where(h => h.Status == "Active").OrderByDescending(h => h.AssignmentDate).FirstOrDefault()

    // Navigation
    public virtual Product Product { get; private set; } = default!;
    public virtual ICollection<AssetAssignmentHistory> AssignmentHistory { get; private set; }
        = new List<AssetAssignmentHistory>();
    public virtual ICollection<AssetReclassificationHistory> ReclassificationHistory { get; private set; }
        = new List<AssetReclassificationHistory>();

    private PhysicalAsset() { }

    private PhysicalAsset(
        Guid id,
        string propertyCode,
        Guid productId,
        string description,
        decimal acquisitionCost,
        DateTime acquisitionDate,
        int estimatedUsefulLife,
        int quantity,
        string unitOfMeasure,
        string? serialNumber,
        string? modelNumber,
        string? location,
        string? ppeType,
        PropertyClassification classification)
    {
        Id = id;
        PropertyCode = propertyCode;
        ProductId = productId;
        Description = description;
        AcquisitionCost = acquisitionCost;
        AcquisitionDate = acquisitionDate;
        EstimatedUsefulLife = estimatedUsefulLife;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        SerialNumber = serialNumber;
        ModelNumber = modelNumber;
        Location = location;
        PPEType = ppeType;
        CurrentClassification = classification;
        AccumulatedDepreciation = 0;

        QueueDomainEvent(new PhysicalAssetCreated { PhysicalAsset = this });
    }

    public static PhysicalAsset Create(
        PropertyClassification classification,
        string propertyCode,
        Guid productId,
        string description,
        decimal acquisitionCost,
        DateTime acquisitionDate,
        int estimatedUsefulLife,
        int quantity = 1,
        string unitOfMeasure = "piece",
        string? serialNumber = null,
        string? modelNumber = null,
        string? location = null,
        string? ppeType = null)
    {
        if (acquisitionCost <= 0)
            throw new ArgumentException("Acquisition cost must be greater than zero.");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        if (estimatedUsefulLife <= 0)
            throw new ArgumentException("Estimated useful life must be greater than zero.");

        return new PhysicalAsset(
            Guid.NewGuid(),
            propertyCode,
            productId,
            description,
            acquisitionCost,
            acquisitionDate,
            estimatedUsefulLife,
            quantity,
            unitOfMeasure,
            serialNumber,
            modelNumber,
            location,
            ppeType,
            classification);
    }

    /// <summary>
    /// Get RCA account code based on current classification
    /// </summary>
    private string GetRCAAccountCode()
    {
        return CurrentClassification switch
        {
            PropertyClassification.Consumable => ValueObjects.RCAAccountCode.SuppliesAndMaterialsInventory,
            PropertyClassification.SemiExpendable => ValueObjects.RCAAccountCode.SemiExpendablePropertyInventory,
            PropertyClassification.PropertyPlantEquipment => GetPPEAccountCode(),
            _ => throw new InvalidOperationException("Unknown classification")
        };
    }

    private string GetPPEAccountCode()
    {
        if (string.IsNullOrWhiteSpace(PPEType))
            return ValueObjects.RCAAccountCode.OtherPropertyPlantAndEquipment;

        return PPEType.ToLowerInvariant() switch
        {
            "machinery" or "equipment" => ValueObjects.RCAAccountCode.MachineryAndEquipment,
            "transportation" or "vehicle" => ValueObjects.RCAAccountCode.TransportationEquipment,
            "furniture" or "fixtures" => ValueObjects.RCAAccountCode.FurnitureFixturesAndBooksEquipment,
            "ict" or "computer" => ValueObjects.RCAAccountCode.ICTEquipment,
            _ => ValueObjects.RCAAccountCode.OtherPropertyPlantAndEquipment
        };
    }

    /// <summary>
    /// Reclassify asset when COA/DBM changes thresholds
    /// Records reclassification history for audit trail
    /// </summary>
    public void Reclassify(PropertyClassification newClassification, string reason, DateTime effectiveDate)
    {
        var oldClassification = CurrentClassification;

        if (oldClassification == newClassification)
            return; // No change needed

        // Record reclassification history
        var history = AssetReclassificationHistory.Create(
            Id,
            PropertyCode,
            oldClassification,
            newClassification,
            effectiveDate,
            reason,
            AcquisitionCost);

        ((List<AssetReclassificationHistory>)ReclassificationHistory).Add(history);

        PreviousClassification = oldClassification;
        CurrentClassification = newClassification;
        LastReclassificationDate = effectiveDate;
        ReclassificationReason = reason;

        // Adjust PPE-specific fields based on new classification
        if (newClassification != PropertyClassification.PropertyPlantEquipment)
        {
            // Downgraded from PPE - clear PPE fields
            PPEType = null;
            // Keep AccumulatedDepreciation for historical record
        }

        QueueDomainEvent(new PhysicalAssetReclassified
        {
            PhysicalAsset = this,
            OldClassification = oldClassification,
            NewClassification = newClassification,
            Reason = reason,
            EffectiveDate = effectiveDate
        });
    }

    /// <summary>
    /// Issue asset via ICS (Semi-Expendable) or PAR (PPE)
    /// Automatically uses correct document type based on classification
    /// </summary>
    public AssetAssignmentHistory Issue(
        Guid employeeId,
        string employeeName,
        string documentNumber,
        int? quantityIssued = null)
    {
        // Check for active assignment
        var activeAssignment = ((List<AssetAssignmentHistory>)AssignmentHistory)
            .FirstOrDefault(h => h.Status == "Active");

        if (activeAssignment != null)
            throw new InvalidOperationException("Asset is already assigned. Use Transfer instead.");

        if (DisposalDate.HasValue)
            throw new InvalidOperationException("Cannot issue disposed asset.");

        var classification = CurrentClassification;
        var docType = classification == PropertyClassification.PropertyPlantEquipment
            ? DocumentType.PAR
            : DocumentType.ICS;

        // Validate quantity for semi-expendable
        if (classification == PropertyClassification.SemiExpendable)
        {
            if (!quantityIssued.HasValue || quantityIssued <= 0)
                throw new ArgumentException("Quantity issued is required for semi-expendable items.");
            if (quantityIssued > Quantity)
                throw new InvalidOperationException("Insufficient stock available.");

            Quantity -= quantityIssued.Value;
        }

        var assignmentDate = DateTime.UtcNow;

        // Create assignment history (no denormalized fields to update)
        var history = AssetAssignmentHistory.CreateInitialAssignment(
            Id,
            PropertyCode,
            employeeId,
            employeeName,
            documentNumber,
            docType,
            assignmentDate,
            quantityIssued ?? 1,
            classification);

        ((List<AssetAssignmentHistory>)AssignmentHistory).Add(history);

        QueueDomainEvent(new PhysicalAssetIssued
        {
            PhysicalAsset = this,
            EmployeeId = employeeId,
            DocumentNumber = documentNumber,
            DocumentType = docType,
            Quantity = quantityIssued ?? 1
        });

        return history;
    }

    /// <summary>
    /// Return asset from employee (works for both ICS and PAR)
    /// </summary>
    public void Return(string reason, string condition, Guid acceptedBy, int? quantityReturned = null)
    {
        // Find active assignment
        var currentAssignment = ((List<AssetAssignmentHistory>)AssignmentHistory)
            .FirstOrDefault(h => h.Status == "Active");

        if (currentAssignment == null)
            throw new InvalidOperationException("Asset is not currently assigned.");

        var classification = CurrentClassification;
        var returnDate = DateTime.UtcNow;

        // Validate quantity for semi-expendable
        if (classification == PropertyClassification.SemiExpendable)
        {
            if (!quantityReturned.HasValue || quantityReturned <= 0)
                throw new ArgumentException("Quantity returned is required for semi-expendable items.");

            Quantity += quantityReturned.Value;
        }

        // Mark assignment as returned
        currentAssignment.MarkAsReturned(returnDate, reason, condition, acceptedBy);

        // Update condition
        Condition = condition;

        QueueDomainEvent(new PhysicalAssetReturned
        {
            PhysicalAsset = this,
            Reason = reason,
            Condition = condition,
            QuantityReturned = quantityReturned ?? 1
        });
    }

    /// <summary>
    /// Record depreciation (only applicable for PPE classification)
    /// </summary>
    public void RecordDepreciation(decimal amount, DateTime depreciationDate)
    {
        if (CurrentClassification != PropertyClassification.PropertyPlantEquipment)
            throw new InvalidOperationException("Only PPE classification can be depreciated.");

        if (amount <= 0)
            throw new ArgumentException("Depreciation amount must be greater than zero.");
        if (DisposalDate.HasValue)
            throw new InvalidOperationException("Cannot depreciate disposed asset.");
        if (AccumulatedDepreciation + amount > AcquisitionCost)
            throw new InvalidOperationException("Accumulated depreciation cannot exceed acquisition cost.");

        AccumulatedDepreciation += amount;

        QueueDomainEvent(new PhysicalAssetDepreciated
        {
            PhysicalAsset = this,
            Amount = amount,
            DepreciationDate = depreciationDate,
            AccumulatedDepreciation = AccumulatedDepreciation,
            BookValue = BookValue
        });
    }

    public void UpdateCondition(string condition, string? remarks = null)
    {
        if (string.IsNullOrWhiteSpace(condition))
            throw new ArgumentException("Condition is required.");
        if (!IsValidCondition(condition))
            throw new ArgumentException("Invalid condition. Must be Good, Fair, or Poor.");

        Condition = condition;

        QueueDomainEvent(new PhysicalAssetConditionUpdated
        {
            PhysicalAsset = this,
            Condition = condition,
            Remarks = remarks
        });
    }

    private static bool IsValidCondition(string condition)
    {
        return condition.Equals("Good", StringComparison.OrdinalIgnoreCase) ||
               condition.Equals("Fair", StringComparison.OrdinalIgnoreCase) ||
               condition.Equals("Poor", StringComparison.OrdinalIgnoreCase);
    }
}