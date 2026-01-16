using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents the budget breakdown for a procurement project.
/// Owned by ProcurementProject aggregate.
/// Invariant: TotalAmount = MooeAmount + CoAmount
/// </summary>
public sealed class ProjectBudget : BaseEntity
{
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Total budget amount (must equal MOOE + CO)
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Maintenance and Other Operating Expenses amount
    /// </summary>
    public decimal MooeAmount { get; private set; }

    /// <summary>
    /// Capital Outlay amount
    /// </summary>
    public decimal CoAmount { get; private set; }

    private ProjectBudget() { }

    internal ProjectBudget(Guid projectId)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        TotalAmount = 0;
        MooeAmount = 0;
        CoAmount = 0;
    }

    internal void Update(decimal totalAmount, decimal mooeAmount, decimal coAmount)
    {
        ValidateAmounts(totalAmount, mooeAmount, coAmount);

        TotalAmount = totalAmount;
        MooeAmount = mooeAmount;
        CoAmount = coAmount;
    }

    private static void ValidateAmounts(decimal totalAmount, decimal mooeAmount, decimal coAmount)
    {
        if (totalAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount cannot be negative.");
        if (mooeAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(mooeAmount), "MOOE amount cannot be negative.");
        if (coAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(coAmount), "CO amount cannot be negative.");

        var calculatedTotal = mooeAmount + coAmount;
        if (totalAmount != calculatedTotal)
            throw new InvalidOperationException($"Total amount ({totalAmount}) must equal MOOE ({mooeAmount}) + CO ({coAmount}) = {calculatedTotal}.");
    }
}
