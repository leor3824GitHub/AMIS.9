using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Fixed asset lifecycle status following Oracle NetSuite Fixed Assets and FASB/IFRS standards.
/// Tracks the complete lifecycle of capital assets from acquisition to disposal.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssetStatus
{
    /// <summary>
    /// Asset on order but not yet received
    /// </summary>
    [Description("On Order")]
    OnOrder = 0,

    /// <summary>
    /// Asset received but not yet placed in service
    /// </summary>
    [Description("In Transit")]
    InTransit = 1,

    /// <summary>
    /// Asset being installed or configured
    /// </summary>
    [Description("Under Installation")]
    UnderInstallation = 2,

    /// <summary>
    /// Asset in active use and being depreciated
    /// </summary>
    [Description("In Service")]
    InService = 3,

    /// <summary>
    /// Asset temporarily not in use but maintained
    /// </summary>
    [Description("Idle")]
    Idle = 4,

    /// <summary>
    /// Asset undergoing maintenance or repairs
    /// </summary>
    [Description("Under Maintenance")]
    UnderMaintenance = 5,

    /// <summary>
    /// Asset out for repair at vendor facility
    /// </summary>
    [Description("Out for Repair")]
    OutForRepair = 6,

    /// <summary>
    /// Asset on loan or lease to another party
    /// </summary>
    [Description("Loaned Out")]
    LoanedOut = 7,

    /// <summary>
    /// Asset leased from another party
    /// </summary>
    [Description("Leased")]
    Leased = 8,

    /// <summary>
    /// Asset marked for disposal but still on books
    /// </summary>
    [Description("Pending Disposal")]
    PendingDisposal = 9,

    /// <summary>
    /// Asset disposed of or sold
    /// </summary>
    [Description("Disposed")]
    Disposed = 10,

    /// <summary>
    /// Asset stolen or lost
    /// </summary>
    [Description("Lost/Stolen")]
    LostOrStolen = 11,

    /// <summary>
    /// Asset damaged beyond economical repair
    /// </summary>
    [Description("Damaged")]
    Damaged = 12,

    /// <summary>
    /// Asset fully depreciated but still in use
    /// </summary>
    [Description("Fully Depreciated")]
    FullyDepreciated = 13,

    /// <summary>
    /// Asset retired from active use
    /// </summary>
    [Description("Retired")]
    Retired = 14,

    /// <summary>
    /// Asset donated to charity or other entity
    /// </summary>
    [Description("Donated")]
    Donated = 15,

    /// <summary>
    /// Asset scrapped or destroyed
    /// </summary>
    [Description("Scrapped")]
    Scrapped = 16,

    /// <summary>
    /// Asset transferred to another location or department
    /// </summary>
    [Description("In Transfer")]
    InTransfer = 17,

    /// <summary>
    /// Asset under construction (for self-constructed assets)
    /// </summary>
    [Description("Under Construction")]
    UnderConstruction = 18,

    /// <summary>
    /// Asset held for sale
    /// </summary>
    [Description("Held for Sale")]
    HeldForSale = 19,

    /// <summary>
    /// Asset in storage - not currently deployed
    /// </summary>
    [Description("In Storage")]
    InStorage = 20,

    /// <summary>
    /// Asset pending insurance claim
    /// </summary>
    [Description("Insurance Claim")]
    InsuranceClaim = 21,

    /// <summary>
    /// Asset undergoing upgrade or improvement
    /// </summary>
    [Description("Under Upgrade")]
    UnderUpgrade = 22
}
