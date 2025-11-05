using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Purchase order line-item inspection status following quality control standards.
/// Represents the quality disposition for each PO line item during receiving inspection.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseItemInspectionStatus
{
    /// <summary>
    /// Item not yet inspected - awaiting quality review
    /// </summary>
    [Description("Not Inspected")]
    NotInspected = 0,

    /// <summary>
    /// Item passed all quality inspections - approved for acceptance
    /// </summary>
    [Description("Passed")]
    Passed = 1,

    /// <summary>
    /// Item failed quality inspection - cannot be accepted
    /// </summary>
    [Description("Failed")]
    Failed = 2,

    /// <summary>
    /// Mixed results - some units passed, some failed
    /// </summary>
    [Description("Partially Passed")]
    PartiallyPassed = 3,

    /// <summary>
    /// Entire lot/batch rejected
    /// </summary>
    [Description("Rejected")]
    Rejected = 4,

    /// <summary>
    /// Inspection in progress
    /// </summary>
    [Description("In Progress")]
    InProgress = 5,

    /// <summary>
    /// Items placed in quarantine pending disposition
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 6,

    /// <summary>
    /// Inspection on hold - awaiting additional information
    /// </summary>
    [Description("On Hold")]
    OnHold = 7,

    /// <summary>
    /// Requires re-inspection after corrective action
    /// </summary>
    [Description("Re-Inspection Required")]
    ReInspectionRequired = 8,

    /// <summary>
    /// Accepted with documented deviations (MRB approved)
    /// </summary>
    [Description("Accepted with Deviation")]
    AcceptedWithDeviation = 9,

    /// <summary>
    /// Skip inspection - certified vendor/trusted supplier
    /// </summary>
    [Description("Skip Inspection")]
    SkipInspection = 10,

    /// <summary>
    /// Awaiting sample testing results
    /// </summary>
    [Description("Pending Lab Test")]
    PendingLabTest = 11,

    /// <summary>
    /// Conditionally approved pending final verification
    /// </summary>
    [Description("Conditionally Approved")]
    ConditionallyApproved = 12
}
