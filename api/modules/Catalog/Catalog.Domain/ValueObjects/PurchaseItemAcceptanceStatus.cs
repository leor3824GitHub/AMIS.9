using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Line-item level acceptance status for purchase order items following Oracle NetSuite standards.
/// Tracks individual item acceptance disposition at the PO line level.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseItemAcceptanceStatus
{
    /// <summary>
    /// Item awaiting receipt and acceptance
    /// </summary>
    [Description("Pending Receipt")]
    Pending = 0,

    /// <summary>
    /// Item fully accepted and posted to inventory
    /// </summary>
    [Description("Accepted")]
    Accepted = 1,

    /// <summary>
    /// Item rejected and returned to vendor
    /// </summary>
    [Description("Rejected")]
    Rejected = 2,

    /// <summary>
    /// Some quantity accepted, remaining quantity pending
    /// </summary>
    [Description("Partially Accepted")]
    PartiallyAccepted = 3,

    /// <summary>
    /// Awaiting quality inspection results
    /// </summary>
    [Description("Pending Inspection")]
    PendingInspection = 4,

    /// <summary>
    /// In quarantine pending disposition decision
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 5,

    /// <summary>
    /// On hold due to discrepancies or issues
    /// </summary>
    [Description("On Hold")]
    OnHold = 6,

    /// <summary>
    /// Returned to vendor for credit or replacement
    /// </summary>
    [Description("Returned to Vendor")]
    ReturnedToVendor = 7,

    /// <summary>
    /// Cancelled before receipt
    /// </summary>
    [Description("Cancelled")]
    Cancelled = 8,

    /// <summary>
    /// Over-received - quantity exceeds PO line quantity
    /// </summary>
    [Description("Over Receipt")]
    OverReceipt = 9,

    /// <summary>
    /// Short shipment - quantity less than expected
    /// </summary>
    [Description("Short Receipt")]
    ShortReceipt = 10,

    /// <summary>
    /// Accepted with deviations (Material Review Board approved)
    /// </summary>
    [Description("Accepted with Deviation")]
    AcceptedWithDeviation = 11
}

