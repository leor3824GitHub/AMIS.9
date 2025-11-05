using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Inventory stock status following Oracle NetSuite inventory management standards.
/// Represents the availability and quality status of inventory items.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StockStatus
{
    /// <summary>
    /// Available for sale or use - good quality stock
    /// </summary>
    [Description("Available")]
    Available = 0,

    /// <summary>
    /// Reserved for specific orders or projects
    /// </summary>
    [Description("Reserved")]
    Reserved = 1,

    /// <summary>
    /// In quarantine pending quality inspection or approval
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 2,

    /// <summary>
    /// On hold - cannot be used or sold
    /// </summary>
    [Description("On Hold")]
    OnHold = 3,

    /// <summary>
    /// Damaged goods - requires disposition decision
    /// </summary>
    [Description("Damaged")]
    Damaged = 4,

    /// <summary>
    /// Obsolete - marked for disposal or write-off
    /// </summary>
    [Description("Obsolete")]
    Obsolete = 5,

    /// <summary>
    /// In transit between locations
    /// </summary>
    [Description("In Transit")]
    InTransit = 6,

    /// <summary>
    /// Currently being picked for an order
    /// </summary>
    [Description("Picking in Progress")]
    Picking = 7,

    /// <summary>
    /// Consignment stock - owned by vendor, in customer location
    /// </summary>
    [Description("Consignment")]
    Consignment = 8,

    /// <summary>
    /// Awaiting return to vendor
    /// </summary>
    [Description("Pending Return")]
    PendingReturn = 9,

    /// <summary>
    /// Committed to production or assembly
    /// </summary>
    [Description("Allocated to Production")]
    AllocatedToProduction = 10,

    /// <summary>
    /// Awaiting receipt - ordered but not yet received
    /// </summary>
    [Description("On Order")]
    OnOrder = 11,

    /// <summary>
    /// Below minimum stock level - reorder needed
    /// </summary>
    [Description("Below Reorder Point")]
    BelowReorderPoint = 12,

    /// <summary>
    /// Out of stock - zero quantity available
    /// </summary>
    [Description("Out of Stock")]
    OutOfStock = 13,

    /// <summary>
    /// Available but nearing expiration date
    /// </summary>
    [Description("Near Expiry")]
    NearExpiry = 14,

    /// <summary>
    /// Expired - cannot be used
    /// </summary>
    [Description("Expired")]
    Expired = 15,

    /// <summary>
    /// Under cycle count or physical inventory
    /// </summary>
    [Description("Under Count")]
    UnderCount = 16
}
