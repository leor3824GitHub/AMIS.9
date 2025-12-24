using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Service for auto-generating Journal Entry Vouchers (JEV) per COA Circular 2022-002
/// </summary>
public interface IJournalEntryService
{
    /// <summary>
    /// Generate JEV for consumable item receipt
    /// Dr: Supplies and Materials Inventory (10501000)
    /// Cr: Accounts Payable / Cash
    /// </summary>
    Task<JournalEntryVoucher> GenerateConsumableReceiptJEV(
        Guid productId,
        decimal amount,
        string poNumber,
        DateTime transactionDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate JEV for consumable item issuance (RSMI)
    /// Dr: Supplies and Materials Expense (50203010)
    /// Cr: Supplies and Materials Inventory (10501000)
    /// </summary>
    Task<JournalEntryVoucher> GenerateConsumableIssuanceJEV(
        Guid productId,
        decimal amount,
        string rsmiNumber,
        DateTime transactionDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate JEV for semi-expendable item receipt
    /// Dr: Semi-Expendable Property Inventory (10599020)
    /// Cr: Accounts Payable / Cash
    /// </summary>
    Task<JournalEntryVoucher> GenerateSemiExpendableReceiptJEV(
        Guid productId,
        decimal amount,
        string poNumber,
        DateTime transactionDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate JEV for semi-expendable item issuance (ICS)
    /// Dr: Semi-Expendable Property Expense (50299010)
    /// Cr: Semi-Expendable Property Inventory (10599020)
    /// </summary>
    Task<JournalEntryVoucher> GenerateSemiExpendableIssuanceJEV(
        Guid productId,
        decimal amount,
        string icsNumber,
        Guid custodianId,
        DateTime transactionDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate JEV for PPE acquisition (PAR)
    /// Dr: PPE Asset Account (1-06-xx-xxx)
    /// Cr: Accounts Payable / Cash
    /// </summary>
    Task<JournalEntryVoucher> GeneratePPEAcquisitionJEV(
        Guid productId,
        decimal amount,
        string parNumber,
        string ppeAccountCode,
        DateTime transactionDate,
        CancellationToken cancellationToken = default);
}
