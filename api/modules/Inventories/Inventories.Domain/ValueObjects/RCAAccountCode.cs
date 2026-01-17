namespace AMIS.WebApi.Inventories.Domain.ValueObjects;

/// <summary>
/// Revised Chart of Accounts (RCA) 2019 account codes per COA Circular 2022-002
/// </summary>
public static class RCAAccountCode
{
    // Consumables (Supplies)
    public const string SuppliesAndMaterialsInventory = "10501000";
    public const string SuppliesAndMaterialsExpense = "50203010";

    // Semi-Expendables
    public const string SemiExpendablePropertyInventory = "10599020";
    public const string SemiExpendablePropertyExpense = "50299010";

    // PPE Base Accounts
    public const string MachineryAndEquipment = "10604010";
    public const string TransportationEquipment = "10605010";
    public const string FurnitureFixturesAndBooksEquipment = "10606010";
    public const string ICTEquipment = "10607010";
    public const string OtherPropertyPlantAndEquipment = "10699990";

    // Accumulated Depreciation
    public const string AccumulatedDepreciation = "10699010";

    // Accounts Payable
    public const string AccountsPayable = "20101010";
}

