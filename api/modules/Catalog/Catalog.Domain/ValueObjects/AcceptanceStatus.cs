using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Goods receipt and acceptance statuses following Oracle NetSuite receiving workflow.
/// Represents the final stage of procurement where inspected goods are formally accepted into inventory.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AcceptanceStatus
{
    /// <summary>
    /// Goods received and awaiting acceptance processing
    /// </summary>
    [Description("Pending Acceptance")]
    Pending = 0,

    /// <summary>
    /// Goods have been accepted and posted to inventory
    /// </summary>
    [Description("Posted to Inventory")]
    Posted = 1,

    /// <summary>
    /// Acceptance transaction cancelled before posting
    /// </summary>
    [Description("Cancelled")]
    Cancelled = 2,

    /// <summary>
    /// Partial acceptance - some items accepted, others pending
    /// </summary>
    [Description("Partially Posted")]
    PartiallyPosted = 3,

    /// <summary>
    /// On hold pending resolution of discrepancies
    /// </summary>
    [Description("On Hold")]
    OnHold = 4,

    /// <summary>
    /// Rejected and returned to vendor
    /// </summary>
    [Description("Rejected - Returned")]
    Rejected = 5,

    /// <summary>
    /// Awaiting quality inspection before acceptance
    /// </summary>
    [Description("Pending Inspection")]
    PendingInspection = 6,

    /// <summary>
    /// In quarantine pending disposition
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 7,

    /// <summary>
    /// Acceptance approved, awaiting final posting
    /// </summary>
    [Description("Approved - Pending Post")]
    Approved = 8,

    /// <summary>
    /// Goods are in receiving area but not yet processed
    /// </summary>
    [Description("In Receiving")]
    InReceiving = 9,

    /// <summary>
    /// Fully completed and closed
    /// </summary>
    [Description("Closed")]
    Closed = 10
}
