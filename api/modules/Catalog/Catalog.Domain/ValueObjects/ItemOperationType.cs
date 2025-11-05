using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Line item operation types for order processing and inventory management.
/// Tracks changes to order line items throughout the order lifecycle.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemOperationType
{
    /// <summary>
    /// No operation - item unchanged
    /// </summary>
    [Description("None")]
    None = 0,

    /// <summary>
    /// Add new line item to order
    /// </summary>
    [Description("Add")]
    Add = 1,

    /// <summary>
    /// Update existing line item (quantity, price, etc.)
    /// </summary>
    [Description("Update")]
    Update = 2,

    /// <summary>
    /// Remove/delete line item from order
    /// </summary>
    [Description("Remove")]
    Remove = 3,

    /// <summary>
    /// Cancel line item (soft delete, keeps audit trail)
    /// </summary>
    [Description("Cancel")]
    Cancel = 4,

    /// <summary>
    /// Substitute one item for another
    /// </summary>
    [Description("Substitute")]
    Substitute = 5,

    /// <summary>
    /// Return line item (customer return or vendor return)
    /// </summary>
    [Description("Return")]
    Return = 6,

    /// <summary>
    /// Backorder - item not available, will ship later
    /// </summary>
    [Description("Backorder")]
    Backorder = 7,

    /// <summary>
    /// Split line item into multiple lines
    /// </summary>
    [Description("Split")]
    Split = 8,

    /// <summary>
    /// Merge multiple line items into one
    /// </summary>
    [Description("Merge")]
    Merge = 9,

    /// <summary>
    /// Hold line item - temporarily suspend processing
    /// </summary>
    [Description("Hold")]
    Hold = 10,

    /// <summary>
    /// Release line item from hold
    /// </summary>
    [Description("Release")]
    Release = 11
}

