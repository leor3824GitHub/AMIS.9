using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Represents inventory transaction types following Oracle NetSuite and SAP standards.
/// Tracks all inventory movements with proper categorization for financial and operational reporting.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    /// <summary>
    /// Default/unspecified transaction type (should not be used in production)
    /// </summary>
    [Description("None")]
    None = 0,

    /// <summary>
    /// Receipt of goods from purchase order (increases inventory, creates payable)
    /// </summary>
    [Description("Item Receipt from Purchase Order")]
    Purchase = 1,

    /// <summary>
    /// Issue of items for internal use, production, or consumption (decreases inventory)
    /// </summary>
    [Description("Item Issuance")]
    Issuance = 2,

    /// <summary>
    /// Manual adjustment of inventory quantities (positive or negative) for correction
    /// </summary>
    [Description("Inventory Adjustment")]
    Adjustment = 3,

    /// <summary>
    /// Transfer between locations/warehouses (neutral to total inventory)
    /// </summary>
    [Description("Inter-Location Transfer")]
    Transfer = 4,

    /// <summary>
    /// Return of goods to supplier/vendor (decreases inventory, reduces payable)
    /// </summary>
    [Description("Vendor Return")]
    VendorReturn = 5,

    /// <summary>
    /// Return of issued items back to inventory (increases inventory)
    /// </summary>
    [Description("Item Return")]
    ItemReturn = 6,

    /// <summary>
    /// Physical inventory count adjustment (reconciliation)
    /// </summary>
    [Description("Physical Count Adjustment")]
    CycleCount = 7,

    /// <summary>
    /// Write-off of damaged, obsolete, or lost inventory (decreases inventory)
    /// </summary>
    [Description("Inventory Write-Off")]
    WriteOff = 8,

    /// <summary>
    /// Assembly/manufacturing output (component consumption and finished goods receipt)
    /// </summary>
    [Description("Work Order Completion")]
    Assembly = 9,

    /// <summary>
    /// Disassembly of finished goods back to components
    /// </summary>
    [Description("Work Order Disassembly")]
    Disassembly = 10,

    /// <summary>
    /// Goods in transit between locations (staging/placeholder status)
    /// </summary>
    [Description("In-Transit Inventory")]
    InTransit = 11,

    /// <summary>
    /// Reservation of inventory for future orders (committed quantity)
    /// </summary>
    [Description("Inventory Reservation")]
    Reservation = 12,

    /// <summary>
    /// Release of previously reserved inventory
    /// </summary>
    [Description("Reservation Release")]
    ReservationRelease = 13,

    /// <summary>
    /// Initial inventory setup during system implementation
    /// </summary>
    [Description("Beginning Balance")]
    OpeningBalance = 14
}
