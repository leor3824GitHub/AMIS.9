using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Inspection item quality disposition codes following ANSI/ASQC Z1.4 and ISO 2859 standards.
/// Used for lot-by-lot acceptance sampling and quality control decisions.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum InspectionItemStatus
{
    /// <summary>
    /// Item awaiting inspection - no quality determination made
    /// </summary>
    [Description("Not Inspected")]
    NotInspected = 0,

    /// <summary>
    /// Item meets all quality specifications - accept for use
    /// </summary>
    [Description("Passed - Accept")]
    Passed = 1,

    /// <summary>
    /// Item fails to meet quality specifications - reject
    /// </summary>
    [Description("Failed - Reject")]
    Failed = 2,

    /// <summary>
    /// Mixed results - some units passed, some failed in the lot
    /// </summary>
    [Description("Partial Pass")]
    Partial = 3,

    /// <summary>
    /// Complete lot rejection - all items in batch failed
    /// </summary>
    [Description("Lot Rejected")]
    Rejected = 4,

    /// <summary>
    /// Accepted with documented deviations (Material Review Board approval)
    /// </summary>
    [Description("Accepted with Deviation")]
    AcceptedWithDeviation = 5,

    /// <summary>
    /// Placed in quarantine pending disposition decision
    /// </summary>
    [Description("Quarantined")]
    Quarantined = 6,

    /// <summary>
    /// Returned to vendor for rework or replacement
    /// </summary>
    [Description("Return to Vendor")]
    ReturnToVendor = 7,

    /// <summary>
    /// Item will be reworked internally to meet specifications
    /// </summary>
    [Description("Rework Required")]
    ReworkRequired = 8,

    /// <summary>
    /// Use as-is with approval despite minor defects
    /// </summary>
    [Description("Use As-Is")]
    UseAsIs = 9,

    /// <summary>
    /// Scrap/dispose - cannot be reworked economically
    /// </summary>
    [Description("Scrap")]
    Scrap = 10,

    /// <summary>
    /// On hold - awaiting additional testing or information
    /// </summary>
    [Description("On Hold")]
    OnHold = 11,

    /// <summary>
    /// Skip inspection - vendor certified/trusted supplier program
    /// </summary>
    [Description("Skip Inspection")]
    SkipInspection = 12,

    /// <summary>
    /// Conditional acceptance - use pending final verification
    /// </summary>
    [Description("Conditionally Accepted")]
    ConditionallyAccepted = 13
}


