using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Inspection request workflow statuses following Oracle NetSuite Quality Management.
/// Tracks the quality control workflow from request creation to final acceptance.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionRequestStatus
{
    /// <summary>
    /// Request has been created but not yet assigned to an inspector
    /// </summary>
    [Description("Pending Assignment")]
    Pending = 0,

    /// <summary>
    /// Inspector has been assigned to the request
    /// </summary>
    [Description("Assigned to Inspector")]
    Assigned = 1,

    /// <summary>
    /// Inspection is currently being performed
    /// </summary>
    [Description("In Progress")]
    InProgress = 2,

    /// <summary>
    /// Inspection has been completed, results recorded
    /// </summary>
    [Description("Completed")]
    Completed = 3,

    /// <summary>
    /// Inspection completed with at least one failed item requiring disposition
    /// </summary>
    [Description("Failed - Disposition Required")]
    Failed = 4,

    /// <summary>
    /// Some quantities have been accepted but not all approved quantities
    /// </summary>
    [Description("Partially Accepted")]
    PartiallyAccepted = 5,

    /// <summary>
    /// All approved quantities have been accepted and moved to inventory
    /// </summary>
    [Description("Fully Accepted")]
    Accepted = 6,

    /// <summary>
    /// Request cancelled before inspection completion
    /// </summary>
    [Description("Cancelled")]
    Cancelled = 7,

    /// <summary>
    /// Inspection on hold pending additional information or samples
    /// </summary>
    [Description("On Hold")]
    OnHold = 8,

    /// <summary>
    /// Awaiting approval from quality manager or Material Review Board (MRB)
    /// </summary>
    [Description("Pending Approval")]
    PendingApproval = 9,

    /// <summary>
    /// Items quarantined pending disposition decision
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 10,

    /// <summary>
    /// Inspection rejected, items returned to vendor
    /// </summary>
    [Description("Rejected - Returned to Vendor")]
    Rejected = 11,

    /// <summary>
    /// Requires re-inspection after corrective action
    /// </summary>
    [Description("Re-Inspection Required")]
    ReInspection = 12,

    /// <summary>
    /// Expedited inspection request for urgent materials
    /// </summary>
    [Description("Expedited")]
    Expedited = 13,

    /// <summary>
    /// Inspection bypassed for certified vendor/trusted supplier
    /// </summary>
    [Description("Skip Inspection - Auto Approved")]
    SkipInspection = 14
}

