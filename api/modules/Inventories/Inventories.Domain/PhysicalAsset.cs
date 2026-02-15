using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Domain.Services;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Unified Physical Asset entity that handles both Semi-Expendable and PPE assets.
/// Manages asset lifecycle including issuance, returns, transfers, and disposal.
/// Classification is determined dynamically through IAssetClassificationPolicy.
/// </summary>
public class PhysicalAsset : AuditableEntity, IAggregateRoot
{
    // Core Identification
    public string PropertyCode { get; private set; } = default!;
    public Guid ProductId { get; private set; }

    // Acquisition Details
    public decimal AcquisitionCost { get; private set; }
    public DateTime AcquisitionDate { get; private set; }

    // Physical Attributes
    public string? SerialNumber { get; private set; }
    public string? ModelNumber { get; private set; }
    public string Condition { get; private set; } = AssetCondition.Good.Value;

    // Quantity (for Semi-Expendable batch tracking)
    public int Quantity { get; private set; } = 1;

    // Lifecycle
    public DateTime? DisposalDate { get; private init; }
    public string? DisposalReason { get; private init; }

    // PPE-Specific Fields
    public decimal AccumulatedDepreciation { get; private set; }
    public decimal BookValue => AcquisitionCost - AccumulatedDepreciation;

    // QR Code & Identification Support
    public string? QRCodeData { get; private set; } // Base64 encoded QR code image or raw QR data
    public DateTime? QRGeneratedDate { get; private set; } // When QR was generated

    // Asset-Specific Photos
    public List<string> ImagePaths { get; private set; } = new(); // Photos specific to this asset instance

    // Optimistic Concurrency Control
    public int Version { get; private set; } = 0; // Incremented on each modification

    // Computed properties for convenience
    public bool IsDisposed => DisposalDate.HasValue;
    public bool HasQRCode => !string.IsNullOrEmpty(QRCodeData);

    private static readonly IAssetClassificationPolicy DefaultPolicy =
        AssetClassificationPolicyFactory.GetDefault();

    public PropertyClassification CurrentClassification =>
        DefaultPolicy.DetermineClassification(AcquisitionCost);

    // RCAAccountCode is computed from CurrentClassification and DefaultPolicy, not stored
    public string RCAAccountCode => "TBD"; // Placeholder - should be resolved from the classification rule

    // Asset Hierarchy (Parent–Child) - ID only, no navigation
    public Guid? ParentAssetId { get; private set; }

    private PhysicalAsset() { }

    private PhysicalAsset(
        Guid id,
        string propertyCode,
        Guid productId,
        decimal acquisitionCost,
        DateTime acquisitionDate,
        int quantity,
        string? serialNumber,
        string? modelNumber)
    {
        Id = id;
        PropertyCode = propertyCode;
        ProductId = productId;
        AcquisitionCost = acquisitionCost;
        AcquisitionDate = acquisitionDate;
        Quantity = quantity;
        SerialNumber = serialNumber;
        ModelNumber = modelNumber;
        AccumulatedDepreciation = 0;
        Version = 0;

        QueueDomainEvent(new PhysicalAssetCreated { PhysicalAsset = this });
    }

    /// <summary>
    /// Increments the version number for optimistic concurrency control.
    /// Called whenever the aggregate is modified.
    /// </summary>
    private void IncrementVersion()
    {
        Version++;
    }

    public static PhysicalAsset Create(
        string propertyCode,
        Guid productId,
        decimal acquisitionCost,
        DateTime acquisitionDate,
        int quantity = 1,
        string? serialNumber = null,
        string? modelNumber = null)
    {
        ValidateCreate(propertyCode, acquisitionCost, quantity);

        return new PhysicalAsset(
            Guid.NewGuid(),
            propertyCode,
            productId,
            acquisitionCost,
            acquisitionDate,
            quantity,
            serialNumber,
            modelNumber);
    }

    /// <summary>
    /// Get classification dynamically based on acquisition cost using the default policy.
    /// For policy-specific classification, pass an IAssetClassificationPolicy to the business methods.
    /// </summary>
    public PropertyClassification GetCurrentClassification()
    {
        return DefaultPolicy.DetermineClassification(AcquisitionCost);
    }

    /// <summary>
    /// Issue asset via ICS (Semi-Expendable) or PAR (PPE)
    /// Automatically uses correct document type based on classification
    /// NOTE: Assignment tracking moved to separate aggregate - this just validates and emits event
    /// </summary>
    public void Issue(
        Guid employeeId,
        string employeeName,
        string documentNumber,
        int? quantityIssued = null,
        string? location = null,
        bool emitEvent = true,
        IAssetClassificationPolicy? classificationPolicy = null,
        string? issuedByName = null,
        string? receivedByName = null,
        string? approvedByName = null)
    {
        ValidateIssue(employeeId, employeeName, documentNumber);

        if (IsDisposed)
            throw new InvalidOperationException("Cannot issue disposed asset.");

        // Use default policy if none provided
        var policy = classificationPolicy ?? new DefaultAssetClassificationPolicy();
        var classification = policy.DetermineClassification(AcquisitionCost);
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

        if (emitEvent)
        {
            QueueDomainEvent(new PhysicalAssetIssued
            {
                PhysicalAsset = this,
                EmployeeId = employeeId,
                DocumentNumber = documentNumber,
                DocumentType = docType,
                Quantity = quantityIssued ?? 1
            });
        }

        IncrementVersion();
    }

    /// <summary>
    /// Return asset from employee (works for both ICS and PAR)
    /// NOTE: Assignment tracking moved to separate aggregate - this just updates asset state
    /// </summary>
    public void Return(string reason, string condition, Guid acceptedBy, int? quantityReturned = null, IAssetClassificationPolicy? classificationPolicy = null)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Return reason is required.", nameof(reason));
        if (!AssetCondition.TryParse(condition, out var validCondition))
            throw new ArgumentException($"Invalid condition '{condition}'. Valid values are: {string.Join(", ", AssetCondition.GetAllValues())}", nameof(condition));
        if (acceptedBy == Guid.Empty)
            throw new ArgumentException("Accepted by is required.", nameof(acceptedBy));

        // Use default policy if none provided
        var policy = classificationPolicy ?? new DefaultAssetClassificationPolicy();
        var classification = policy.DetermineClassification(AcquisitionCost);

        // Validate quantity for semi-expendable
        if (classification == PropertyClassification.SemiExpendable)
        {
            if (!quantityReturned.HasValue || quantityReturned <= 0)
                throw new ArgumentException("Quantity returned is required for semi-expendable items.");

            Quantity += quantityReturned.Value;
        }

        // Update condition using validated value
        Condition = validCondition!.Value;

        QueueDomainEvent(new PhysicalAssetReturned
        {
            PhysicalAsset = this,
            Reason = reason,
            Condition = condition,
            QuantityReturned = quantityReturned ?? 1
        });

        IncrementVersion();
    }

    /// <summary>
    /// Transfer asset from one employee to another.
    /// NOTE: Assignment tracking moved to separate aggregate - this just validates and emits event
    /// </summary>
    public void Transfer(
        Guid newEmployeeId,
        string newEmployeeName,
        string transferDocumentNumber,
        int? quantityTransferred = null,
        string? location = null,
        bool emitEvent = true,
        IAssetClassificationPolicy? classificationPolicy = null,
        string? transferReason = null)
    {
        ValidateIssue(newEmployeeId, newEmployeeName, transferDocumentNumber);

        if (IsDisposed)
            throw new InvalidOperationException("Cannot transfer disposed asset.");

        // Use default policy if none provided
        var policy = classificationPolicy ?? new DefaultAssetClassificationPolicy();
        var classification = policy.DetermineClassification(AcquisitionCost);

        // Validate quantity for semi-expendable
        if (classification == PropertyClassification.SemiExpendable)
        {
            if (!quantityTransferred.HasValue || quantityTransferred <= 0)
                throw new ArgumentException("Quantity transferred is required for semi-expendable items.");
            if (quantityTransferred > Quantity)
                throw new InvalidOperationException("Insufficient stock available for transfer.");
        }

        var docType = classification == PropertyClassification.PropertyPlantEquipment
            ? DocumentType.PAR
            : DocumentType.ICS;

        if (emitEvent)
        {
            QueueDomainEvent(new PhysicalAssetIssued
            {
                PhysicalAsset = this,
                EmployeeId = newEmployeeId,
                DocumentNumber = transferDocumentNumber,
                DocumentType = docType,
                Quantity = quantityTransferred ?? 1
            });
        }

        IncrementVersion();
    }

    /// <summary>
    /// Disposes of the asset with a reason.
    /// Once disposed, asset cannot be issued, transferred, or used in other operations.
    /// </summary>
    public void Dispose(string disposalReason)
    {
        if (string.IsNullOrWhiteSpace(disposalReason))
            throw new ArgumentException("Disposal reason is required.", nameof(disposalReason));
        if (IsDisposed)
            throw new InvalidOperationException("Asset is already disposed.");

        QueueDomainEvent(new PhysicalAssetDisposed
        {
            PhysicalAsset = this,
            DisposalReason = disposalReason,
            DisposalDate = DateTime.UtcNow
        });

        IncrementVersion();
    }

    /// <summary>
    /// Record depreciation (only applicable for PPE classification).
    /// Classification is determined by IAssetClassificationPolicy.
    /// </summary>
    public void RecordDepreciation(decimal amount, DateTime depreciationDate, IAssetClassificationPolicy? classificationPolicy = null)
    {
        // Use default policy if none provided
        var policy = classificationPolicy ?? new DefaultAssetClassificationPolicy();
        var classification = policy.DetermineClassification(AcquisitionCost);
        if (classification != PropertyClassification.PropertyPlantEquipment)
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

        IncrementVersion();
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

        IncrementVersion();
    }

    /// <summary>
    /// Generates and stores a QR code for the asset.
    /// QR code data should contain property code and asset identification.
    /// </summary>
    public void GenerateQRCode(string qrCodeData)
    {
        if (string.IsNullOrWhiteSpace(qrCodeData))
            throw new ArgumentException("QR code data cannot be empty.", nameof(qrCodeData));

        if (IsDisposed)
            throw new InvalidOperationException("Cannot generate QR code for disposed asset.");

        QRCodeData = qrCodeData;
        QRGeneratedDate = DateTime.UtcNow;

        QueueDomainEvent(new PhysicalAssetQRCodeGenerated
        {
            PhysicalAsset = this,
            GeneratedDate = QRGeneratedDate.Value
        });

        IncrementVersion();
    }

    /// <summary>
    /// Sets the parent asset for this asset (creates a parent-child relationship).
    /// </summary>
    /// <param name="parentAssetId">The ID of the parent asset.</param>
    public void SetParentAsset(Guid parentAssetId)
    {
        if (parentAssetId == Guid.Empty)
            throw new ArgumentException("Parent asset ID must be a valid GUID.", nameof(parentAssetId));
        if (parentAssetId == Id)
            throw new InvalidOperationException("An asset cannot be its own parent.");

        ParentAssetId = parentAssetId;
        IncrementVersion();
    }

    /// <summary>
    /// Clears the parent asset relationship.
    /// </summary>
    public void ClearParentAsset()
    {
        ParentAssetId = null;
        IncrementVersion();
    }

    /// <summary>
    /// Add an image path to this asset's photo collection.
    /// </summary>
    public PhysicalAsset AddImagePath(string imagePath)
    {
        if (!string.IsNullOrWhiteSpace(imagePath) && !ImagePaths.Contains(imagePath))
        {
            ImagePaths.Add(imagePath);
            IncrementVersion();
        }
        return this;
    }

    /// <summary>
    /// Remove a specific image path from this asset's photo collection.
    /// </summary>
    public PhysicalAsset RemoveImagePath(string imagePath)
    {
        if (ImagePaths.Remove(imagePath))
        {
            IncrementVersion();
        }
        return this;
    }

    /// <summary>
    /// Clear all image paths from this asset.
    /// </summary>
    public PhysicalAsset ClearImagePaths()
    {
        if (ImagePaths.Count > 0)
        {
            ImagePaths.Clear();
            IncrementVersion();
        }
        return this;
    }

    private static bool AreImagePathsEqual(List<string> list1, List<string> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        return list1.SequenceEqual(list2);
    }

    private static void ValidateCreate(
        string propertyCode,
        decimal acquisitionCost,
        int quantity)
    {
        if (string.IsNullOrWhiteSpace(propertyCode))
            throw new ArgumentException("Property code is required.", nameof(propertyCode));
        if (acquisitionCost <= 0)
            throw new ArgumentException("Acquisition cost must be greater than zero.", nameof(acquisitionCost));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
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
