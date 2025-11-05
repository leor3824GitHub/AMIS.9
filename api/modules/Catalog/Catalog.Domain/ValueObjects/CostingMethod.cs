using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Inventory costing/valuation methods following GAAP, IFRS, and Oracle NetSuite standards.
/// Determines how inventory costs are calculated and reported for financial statements.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CostingMethod
{
    /// <summary>
    /// Weighted Average Cost - Average cost of all units in inventory
    /// Most commonly used, smooths out price fluctuations
    /// </summary>
    [Description("Weighted Average Cost")]
    WeightedAverage = 0,

    /// <summary>
    /// First-In, First-Out - Oldest costs are expensed first
    /// Allowed under both GAAP and IFRS
    /// </summary>
    [Description("FIFO (First-In, First-Out)")]
    FIFO = 1,

    /// <summary>
    /// Last-In, First-Out - Newest costs are expensed first
    /// Allowed under US GAAP only, not permitted under IFRS
    /// </summary>
    [Description("LIFO (Last-In, First-Out)")]
    LIFO = 2,

    /// <summary>
    /// Specific Identification - Actual cost of specific items
    /// Used for unique, high-value items (vehicles, art, real estate)
    /// </summary>
    [Description("Specific Identification")]
    SpecificIdentification = 3,

    /// <summary>
    /// Standard Cost - Predetermined estimated cost
    /// Used in manufacturing for variance analysis
    /// </summary>
    [Description("Standard Cost")]
    StandardCost = 4,

    /// <summary>
    /// Moving Average - Recalculates average after each purchase
    /// Common in perpetual inventory systems
    /// </summary>
    [Description("Moving Average")]
    MovingAverage = 5,

    /// <summary>
    /// Serial/Lot Tracked - Cost per serial number or lot
    /// Used for serialized or batch-tracked items
    /// </summary>
    [Description("Serial/Lot Cost")]
    SerialLotCost = 6,

    /// <summary>
    /// Actual Cost - Exact purchase cost for each item
    /// Most accurate but administratively intensive
    /// </summary>
    [Description("Actual Cost")]
    ActualCost = 7,

    /// <summary>
    /// Latest Purchase Price - Most recent purchase cost
    /// Simple method for fast-moving inventory
    /// </summary>
    [Description("Latest Purchase Price")]
    LatestPurchasePrice = 8,

    /// <summary>
    /// Landed Cost - Purchase price plus all acquisition costs
    /// Includes freight, duties, handling, insurance
    /// </summary>
    [Description("Landed Cost")]
    LandedCost = 9,

    /// <summary>
    /// Replacement Cost - Current market cost to replace item
    /// Used for lower of cost or market (LCM) valuations
    /// </summary>
    [Description("Replacement Cost")]
    ReplacementCost = 10
}
