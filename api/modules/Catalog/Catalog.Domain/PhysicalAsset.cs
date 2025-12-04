using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.Services;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Unified Physical Asset entity that handles both Semi-Expendable and PPE
/// Classification is dynamic based on configurable thresholds from AssetClassificationRule
/// Supports automatic reclassification when COA/DBM changes thresholds
/// Refactored to use domain services for validation and database-driven configuration
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
    public string Condition { get; private set; } = AssetCondition.Good.Value;

    // Quantity (for Semi-Expendable batch tracking)
    public int Quantity { get; private set; } = 1;
    public string UnitOfMeasure { get; private set; } = default!; // Set from UnitOfMeasure configuration

    // Lifecycle
    public int EstimatedUsefulLife { get; private set; } // in months
    public DateTime? DisposalDate { get; private init; }
    public string? DisposalReason { get; private init; }

    // Classification (determined at creation/update using IAssetClassificationService)
    public PropertyClassification CurrentClassification { get; private set; }

    // PPE-Specific Fields (null if Semi-Expendable)
    public string? PPEType { get; private set; } // Machinery, ICT, etc.
    public decimal AccumulatedDepreciation { get; private set; }
    public decimal BookValue => AcquisitionCost - AccumulatedDepreciation;

    // Dynamic RCA Account (calculated based on current classification)
    public string RCAAccountCode => GetRCAAccountCode();

    // Computed properties for convenience
    public bool IsDisposed => DisposalDate.HasValue;
    public bool IsDepreciable => CurrentClassification == PropertyClassification.PropertyPlantEquipment;
    public AssetAssignmentHistory? CurrentAssignment =>
        AssignmentHistory.FirstOrDefault(h => h.Status == "Active");
    public AssetReclassificationHistory? LastReclassification =>
        ReclassificationHistory.OrderByDescending(h => h.EffectiveDate).FirstOrDefault();

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
        ValidateCreate(propertyCode, description, acquisitionCost, quantity, estimatedUsefulLife, unitOfMeasure, classification, ppeType);

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
    /// Note: For PPE, the account code should be retrieved from PPETypeDefinition using IPPETypeService
    /// This method provides a basic fallback when service is not available
    /// </summary>
    private string GetRCAAccountCode()
    {
        return CurrentClassification switch
        {
            PropertyClassification.Consumable => ValueObjects.RCAAccountCode.SuppliesAndMaterialsInventory,
            PropertyClassification.SemiExpendable => ValueObjects.RCAAccountCode.SemiExpendablePropertyInventory,
            PropertyClassification.PropertyPlantEquipment => GetPPEAccountCodeFallback(),
            _ => throw new InvalidOperationException("Unknown classification")
        };
    }

    /// <summary>
    /// Fallback PPE account code logic when domain service is not available
    /// In production, use IPPETypeService.GetRCAAccountCodeAsync() for database-driven mapping
    /// </summary>
    private string GetPPEAccountCodeFallback()
    {
        if (string.IsNullOrWhiteSpace(PPEType))
            return ValueObjects.RCAAccountCode.OtherPropertyPlantAndEquipment;

        // Basic fallback - in practice, query PPETypeDefinition table
        return PPEType.ToUpperInvariant() switch
        {
            "MACHINERY" or "EQUIPMENT" => ValueObjects.RCAAccountCode.MachineryAndEquipment,
            "TRANSPORTATION" or "VEHICLE" => ValueObjects.RCAAccountCode.TransportationEquipment,
            "FURNITURE" or "FIXTURES" => ValueObjects.RCAAccountCode.FurnitureFixturesAndBooksEquipment,
            "ICT" or "COMPUTER" => ValueObjects.RCAAccountCode.ICTEquipment,
            _ => ValueObjects.RCAAccountCode.OtherPropertyPlantAndEquipment
        };
    }

    /// <summary>
    /// Reclassify asset when COA/DBM changes thresholds
    /// Records reclassification history for audit trail
    /// </summary>
    public void Reclassify(PropertyClassification newClassification, string reason, DateTime effectiveDate)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reclassification reason is required.", nameof(reason));
        if (IsDisposed)
            throw new InvalidOperationException("Cannot reclassify a disposed asset.");

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

        ReclassificationHistory.Add(history);

        CurrentClassification = newClassification;

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
        ValidateIssue(employeeId, employeeName, documentNumber);

        if (CurrentAssignment != null)
            throw new InvalidOperationException("Asset is already assigned. Use Transfer instead.");

        if (IsDisposed)
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

        AssignmentHistory.Add(history);

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
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Return reason is required.", nameof(reason));
        if (!AssetCondition.TryParse(condition, out var validCondition))
            throw new ArgumentException($"Invalid condition '{condition}'. Valid values are: {string.Join(", ", AssetCondition.GetAllValues())}", nameof(condition));
        if (acceptedBy == Guid.Empty)
            throw new ArgumentException("Accepted by is required.", nameof(acceptedBy));

        var currentAssignment = CurrentAssignment;
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

        // Update condition using validated value
        Condition = validCondition!.Value;

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
        if (!IsDepreciable)
            throw new InvalidOperationException("Only PPE classification can be depreciated.");
        if (amount <= 0)
            throw new ArgumentException("Depreciation amount must be greater than zero.", nameof(amount));
        if (IsDisposed)
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
            throw new ArgumentException("Condition is required.", nameof(condition));
        if (!AssetCondition.TryParse(condition, out var validCondition))
            throw new ArgumentException($"Invalid condition '{condition}'. Valid values are: {string.Join(", ", AssetCondition.GetAllValues())}", nameof(condition));
        if (IsDisposed)
            throw new InvalidOperationException("Cannot update condition of disposed asset.");

        Condition = validCondition!.Value;

        QueueDomainEvent(new PhysicalAssetConditionUpdated
        {
            PhysicalAsset = this,
            Condition = Condition,
            Remarks = remarks
        });
    }

    private static void ValidateCreate(
        string propertyCode,
        string description,
        decimal acquisitionCost,
        int quantity,
        int estimatedUsefulLife,
        string unitOfMeasure,
        PropertyClassification classification,
        string? ppeType)
    {
        if (string.IsNullOrWhiteSpace(propertyCode))
            throw new ArgumentException("Property code is required.", nameof(propertyCode));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));
        if (string.IsNullOrWhiteSpace(unitOfMeasure))
            throw new ArgumentException("Unit of measure is required.", nameof(unitOfMeasure));
        if (acquisitionCost <= 0)
            throw new ArgumentException("Acquisition cost must be greater than zero.", nameof(acquisitionCost));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (estimatedUsefulLife <= 0)
            throw new ArgumentException("Estimated useful life must be greater than zero.", nameof(estimatedUsefulLife));
        if (classification == PropertyClassification.PropertyPlantEquipment && string.IsNullOrWhiteSpace(ppeType))
            throw new ArgumentException("PPE type is required for Property, Plant and Equipment classification.", nameof(ppeType));
    }

    private static void ValidateIssue(Guid employeeId, string employeeName, string documentNumber)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee ID is required.", nameof(employeeId));
        if (string.IsNullOrWhiteSpace(employeeName))
            throw new ArgumentException("Employee name is required.", nameof(employeeName));
        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Document number is required.", nameof(documentNumber));
    }
}