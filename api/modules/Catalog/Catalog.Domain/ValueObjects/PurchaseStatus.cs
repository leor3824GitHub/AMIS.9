using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Purchase Order lifecycle statuses following Oracle NetSuite procurement standards.
/// Represents the complete procurement workflow from requisition to closure.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseStatus
{
    /// <summary>
    /// Purchase order is being created and can still be edited (not submitted to vendor)
    /// </summary>
    [Description("Draft - In Progress")]
    Draft = 0,

    /// <summary>
    /// Purchase order is awaiting internal approval before submission
    /// </summary>
    [Description("Pending Approval")]
    PendingApproval = 1,

    /// <summary>
    /// Purchase order has been approved internally but not yet sent to vendor
    /// </summary>
    [Description("Approved")]
    Approved = 2,

    /// <summary>
    /// Purchase order has been sent to the supplier/vendor
    /// </summary>
    [Description("Submitted to Vendor")]
    Submitted = 3,

    /// <summary>
    /// Vendor has acknowledged receipt and acceptance of the PO
    /// </summary>
    [Description("Vendor Acknowledged")]
    Acknowledged = 4,

    /// <summary>
    /// Vendor is preparing/processing the order
    /// </summary>
    [Description("In Progress")]
    InProgress = 5,

    /// <summary>
    /// Items have been shipped by the vendor but not yet received
    /// </summary>
    [Description("Shipped")]
    Shipped = 6,

    /// <summary>
    /// Some items have been received, others pending
    /// </summary>
    [Description("Partially Received")]
    PartiallyReceived = 7,

    /// <summary>
    /// All items have been physically received (may need inspection/billing)
    /// </summary>
    [Description("Fully Received")]
    FullyReceived = 8,

    /// <summary>
    /// Some items have been delivered after inspection approval
    /// </summary>
    [Description("Partially Delivered")]
    PartiallyDelivered = 9,

    /// <summary>
    /// All items delivered and inspected, ready for invoice matching
    /// </summary>
    [Description("Delivered")]
    Delivered = 10,

    /// <summary>
    /// Invoice received from vendor, awaiting matching and payment
    /// </summary>
    [Description("Pending Invoice")]
    PendingInvoice = 11,

    /// <summary>
    /// Invoice has been matched and approved for payment
    /// </summary>
    [Description("Invoiced")]
    Invoiced = 12,

    /// <summary>
    /// Payment has been processed/scheduled
    /// </summary>
    [Description("Pending Payment")]
    PendingPayment = 13,

    /// <summary>
    /// Fully completed - all items received, invoiced, and paid
    /// </summary>
    [Description("Closed - Completed")]
    Closed = 14,

    /// <summary>
    /// Purchase order cancelled before fulfillment
    /// </summary>
    [Description("Cancelled")]
    Cancelled = 15,

    /// <summary>
    /// On hold due to issues (payment, quality, vendor, etc.)
    /// </summary>
    [Description("On Hold")]
    OnHold = 16,

    /// <summary>
    /// Waiting for budget approval or other prerequisites
    /// </summary>
    [Description("Pending Budget Approval")]
    Pending = 17,

    /// <summary>
    /// Rejected during approval workflow
    /// </summary>
    [Description("Rejected")]
    Rejected = 18
}

