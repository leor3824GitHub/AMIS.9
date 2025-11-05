using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Warehouse location types following Oracle NetSuite WMS (Warehouse Management System) standards.
/// Represents different types of storage locations within a warehouse.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocationType
{
    /// <summary>
    /// Standard storage bin for regular inventory
    /// </summary>
    [Description("Storage Bin")]
    Bin = 0,

    /// <summary>
    /// Pallet storage location
    /// </summary>
    [Description("Pallet Location")]
    Pallet = 1,

    /// <summary>
    /// High-bay racking system
    /// </summary>
    [Description("Rack")]
    Rack = 2,

    /// <summary>
    /// Bulk storage area for large quantities
    /// </summary>
    [Description("Bulk Storage")]
    Bulk = 3,

    /// <summary>
    /// Receiving dock area
    /// </summary>
    [Description("Receiving Dock")]
    ReceivingDock = 4,

    /// <summary>
    /// Shipping dock area
    /// </summary>
    [Description("Shipping Dock")]
    ShippingDock = 5,

    /// <summary>
    /// Quarantine/hold area for inspection
    /// </summary>
    [Description("Quarantine Area")]
    Quarantine = 6,

    /// <summary>
    /// Production/manufacturing floor location
    /// </summary>
    [Description("Production Floor")]
    ProductionFloor = 7,

    /// <summary>
    /// Picking area for order fulfillment
    /// </summary>
    [Description("Pick Zone")]
    PickZone = 8,

    /// <summary>
    /// Staging area for orders being prepared
    /// </summary>
    [Description("Staging Area")]
    StagingArea = 9,

    /// <summary>
    /// Returns processing area
    /// </summary>
    [Description("Returns Area")]
    ReturnsArea = 10,

    /// <summary>
    /// Cold storage/refrigerated area
    /// </summary>
    [Description("Cold Storage")]
    ColdStorage = 11,

    /// <summary>
    /// Hazardous materials storage
    /// </summary>
    [Description("Hazmat Storage")]
    HazmatStorage = 12,

    /// <summary>
    /// High-value items secure storage
    /// </summary>
    [Description("Secure Storage")]
    SecureStorage = 13,

    /// <summary>
    /// Virtual location for non-physical items
    /// </summary>
    [Description("Virtual Location")]
    Virtual = 14,

    /// <summary>
    /// Consignment inventory at customer site
    /// </summary>
    [Description("Consignment Location")]
    Consignment = 15,

    /// <summary>
    /// Third-party logistics warehouse
    /// </summary>
    [Description("3PL Warehouse")]
    ThirdPartyWarehouse = 16,

    /// <summary>
    /// Mobile location (e.g., delivery vehicle)
    /// </summary>
    [Description("Mobile Location")]
    Mobile = 17,

    /// <summary>
    /// Scrap/disposal area
    /// </summary>
    [Description("Scrap Area")]
    ScrapArea = 18,

    /// <summary>
    /// Quality inspection area
    /// </summary>
    [Description("QC Inspection Area")]
    InspectionArea = 19
}
