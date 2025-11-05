using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Quality inspection statuses following Oracle NetSuite Quality Management and ISO 9001 standards.
/// Tracks the complete inspection workflow from initiation to final disposition.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionStatus
{
    /// <summary>
    /// Inspection has been scheduled but not yet started
    /// </summary>
    [Description("Scheduled")]
    Scheduled = 0,

    /// <summary>
    /// Quality inspection is currently being performed
    /// </summary>
    [Description("In Progress")]
    InProgress = 1,

    /// <summary>
    /// Inspection completed, awaiting review and approval decision
    /// </summary>
    [Description("Completed - Pending Review")]
    Completed = 2,

    /// <summary>
    /// Inspection results reviewed and approved - goods can proceed to inventory
    /// </summary>
    [Description("Approved")]
    Approved = 3,

    /// <summary>
    /// Inspection results show failure - goods rejected
    /// </summary>
    [Description("Rejected")]
    Rejected = 4,

    /// <summary>
    /// Inspection cancelled before completion
    /// </summary>
    [Description("Cancelled")]
    Cancelled = 5,

    /// <summary>
    /// Approved with minor deviations documented (Engineering Change Request)
    /// </summary>
    [Description("Conditionally Approved")]
    ConditionallyApproved = 6,

    /// <summary>
    /// Inspection on hold pending additional information or testing
    /// </summary>
    [Description("On Hold")]
    OnHold = 7,

    /// <summary>
    /// Items quarantined pending disposition decision
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 8,

    /// <summary>
    /// Requires re-inspection after corrective action
    /// </summary>
    [Description("Re-Inspection Required")]
    ReInspectionRequired = 9,

    /// <summary>
    /// Partial approval - some items passed, some failed
    /// </summary>
    [Description("Partially Approved")]
    PartiallyApproved = 10
}
