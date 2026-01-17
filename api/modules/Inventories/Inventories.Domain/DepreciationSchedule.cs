using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a monthly depreciation schedule entry for a PPE asset.
/// Tracks depreciation calculations using the Straight-Line Method.
/// Supports monthly posting to accounting records via JEV (Journal Entry Voucher).
/// </summary>
public class DepreciationSchedule : AuditableEntity, IAggregateRoot
{
    public Guid PhysicalAssetId { get; private set; }
    public int Month { get; private set; } // 1-12
    public int Year { get; private set; }
    public decimal MonthlyDepreciationAmount { get; private set; }
    public decimal AccumulatedDepreciationAmount { get; private set; }
    public DepreciationScheduleStatus Status { get; private set; }
    public Guid? JournalEntryVoucherId { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public string? Remarks { get; private set; }

    // Navigation
    public virtual PhysicalAsset PhysicalAsset { get; private set; } = default!;
    public virtual JournalEntryVoucher? JournalEntryVoucher { get; private set; }

    // Computed property
    public DateTime ScheduleDate => new DateTime(Year, Month, 1);

    private DepreciationSchedule() { }

    private DepreciationSchedule(
        Guid id,
        Guid physicalAssetId,
        int month,
        int year,
        decimal monthlyDepreciationAmount,
        decimal accumulatedDepreciationAmount)
    {
        Id = id;
        PhysicalAssetId = physicalAssetId;
        Month = month;
        Year = year;
        MonthlyDepreciationAmount = monthlyDepreciationAmount;
        AccumulatedDepreciationAmount = accumulatedDepreciationAmount;
        Status = DepreciationScheduleStatus.Pending;
    }

    /// <summary>
    /// Creates a depreciation schedule entry for a specific month/year.
    /// </summary>
    public static DepreciationSchedule Create(
        Guid physicalAssetId,
        int month,
        int year,
        decimal monthlyDepreciationAmount,
        decimal accumulatedDepreciationAmount)
    {
        if (physicalAssetId == Guid.Empty)
            throw new ArgumentException("Physical Asset ID must be provided.", nameof(physicalAssetId));

        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12.", nameof(month));

        if (year < 2000 || year > DateTime.UtcNow.Year + 10)
            throw new ArgumentException("Year must be valid and reasonable.", nameof(year));

        if (monthlyDepreciationAmount < 0)
            throw new ArgumentException("Monthly depreciation amount must be non-negative.", nameof(monthlyDepreciationAmount));

        if (accumulatedDepreciationAmount < 0)
            throw new ArgumentException("Accumulated depreciation must be non-negative.", nameof(accumulatedDepreciationAmount));

        return new DepreciationSchedule(
            Guid.NewGuid(),
            physicalAssetId,
            month,
            year,
            monthlyDepreciationAmount,
            accumulatedDepreciationAmount);
    }

    /// <summary>
    /// Posts the depreciation to accounting records via JEV.
    /// </summary>
    public void Post(Guid journalEntryVoucherId)
    {
        if (Status != DepreciationScheduleStatus.Pending)
            throw new InvalidOperationException($"Cannot post depreciation with status {Status}.");

        if (journalEntryVoucherId == Guid.Empty)
            throw new ArgumentException("Journal Entry Voucher ID must be provided.", nameof(journalEntryVoucherId));

        Status = DepreciationScheduleStatus.Posted;
        JournalEntryVoucherId = journalEntryVoucherId;
        PostedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Reverses the depreciation entry (e.g., when asset is disposed).
    /// </summary>
    public void Reverse(string reason = "Asset disposed")
    {
        if (Status == DepreciationScheduleStatus.Reversed)
            throw new InvalidOperationException("Depreciation already reversed.");

        Status = DepreciationScheduleStatus.Reversed;
        Remarks = reason;
    }

    /// <summary>
    /// Checks if this schedule entry is for the current month.
    /// </summary>
    public bool IsCurrentMonth
    {
        get
        {
            var now = DateTime.UtcNow;
            return Month == now.Month && Year == now.Year;
        }
    }

    /// <summary>
    /// Checks if this schedule entry is in the past.
    /// </summary>
    public bool IsPast => ScheduleDate < DateTime.UtcNow.Date;
}

