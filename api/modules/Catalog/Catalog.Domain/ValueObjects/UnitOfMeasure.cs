using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Standard Units of Measure following ISO 80000 and UN/CEFACT standards.
/// Used for inventory, purchasing, and logistics operations.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UnitOfMeasure
{
    // Quantity/Count Units
    /// <summary>
    /// Individual pieces or items (default)
    /// </summary>
    [Description("Piece")]
    Piece = 0,

    /// <summary>
    /// Individual units
    /// </summary>
    [Description("Each")]
    Each = 1,

    /// <summary>
    /// Set of items
    /// </summary>
    [Description("Set")]
    Set = 2,

    /// <summary>
    /// Pair of items
    /// </summary>
    [Description("Pair")]
    Pair = 3,

    /// <summary>
    /// Dozen (12 units)
    /// </summary>
    [Description("Dozen")]
    Dozen = 4,

    // Weight Units
    /// <summary>
    /// Kilogram (1000 grams)
    /// </summary>
    [Description("Kilogram")]
    Kilogram = 10,

    /// <summary>
    /// Gram
    /// </summary>
    [Description("Gram")]
    Gram = 11,

    /// <summary>
    /// Metric Ton (1000 kg)
    /// </summary>
    [Description("Metric Ton")]
    MetricTon = 12,

    /// <summary>
    /// Pound (lb)
    /// </summary>
    [Description("Pound")]
    Pound = 13,

    /// <summary>
    /// Ounce (oz)
    /// </summary>
    [Description("Ounce")]
    Ounce = 14,

    /// <summary>
    /// US Ton (2000 lbs)
    /// </summary>
    [Description("US Ton")]
    Ton = 15,

    // Volume Units
    /// <summary>
    /// Liter
    /// </summary>
    [Description("Liter")]
    Liter = 20,

    /// <summary>
    /// Milliliter
    /// </summary>
    [Description("Milliliter")]
    Milliliter = 21,

    /// <summary>
    /// Cubic Meter
    /// </summary>
    [Description("Cubic Meter")]
    CubicMeter = 22,

    /// <summary>
    /// Gallon (US)
    /// </summary>
    [Description("Gallon")]
    Gallon = 23,

    /// <summary>
    /// Fluid Ounce (US)
    /// </summary>
    [Description("Fluid Ounce")]
    FluidOunce = 24,

    /// <summary>
    /// Barrel (oil barrel = 42 gallons)
    /// </summary>
    [Description("Barrel")]
    Barrel = 25,

    // Length Units
    /// <summary>
    /// Meter
    /// </summary>
    [Description("Meter")]
    Meter = 30,

    /// <summary>
    /// Centimeter
    /// </summary>
    [Description("Centimeter")]
    Centimeter = 31,

    /// <summary>
    /// Millimeter
    /// </summary>
    [Description("Millimeter")]
    Millimeter = 32,

    /// <summary>
    /// Kilometer
    /// </summary>
    [Description("Kilometer")]
    Kilometer = 33,

    /// <summary>
    /// Foot
    /// </summary>
    [Description("Foot")]
    Foot = 34,

    /// <summary>
    /// Inch
    /// </summary>
    [Description("Inch")]
    Inch = 35,

    /// <summary>
    /// Yard
    /// </summary>
    [Description("Yard")]
    Yard = 36,

    // Area Units
    /// <summary>
    /// Square Meter
    /// </summary>
    [Description("Square Meter")]
    SquareMeter = 40,

    /// <summary>
    /// Square Foot
    /// </summary>
    [Description("Square Foot")]
    SquareFoot = 41,

    // Packaging Units
    /// <summary>
    /// Box or carton
    /// </summary>
    [Description("Box")]
    Box = 50,

    /// <summary>
    /// Case (multiple boxes)
    /// </summary>
    [Description("Case")]
    Case = 51,

    /// <summary>
    /// Pallet
    /// </summary>
    [Description("Pallet")]
    Pallet = 52,

    /// <summary>
    /// Container (shipping container)
    /// </summary>
    [Description("Container")]
    Container = 53,

    /// <summary>
    /// Bag or sack
    /// </summary>
    [Description("Bag")]
    Bag = 54,

    /// <summary>
    /// Drum (cylindrical container)
    /// </summary>
    [Description("Drum")]
    Drum = 55,

    /// <summary>
    /// Bundle
    /// </summary>
    [Description("Bundle")]
    Bundle = 56,

    /// <summary>
    /// Roll
    /// </summary>
    [Description("Roll")]
    Roll = 57,

    /// <summary>
    /// Reel (for cable, wire, etc.)
    /// </summary>
    [Description("Reel")]
    Reel = 58,

    /// <summary>
    /// Carton
    /// </summary>
    [Description("Carton")]
    Carton = 59,

    /// <summary>
    /// Crate
    /// </summary>
    [Description("Crate")]
    Crate = 60,

    // Time Units
    /// <summary>
    /// Hour (for services or labor)
    /// </summary>
    [Description("Hour")]
    Hour = 70,

    /// <summary>
    /// Day (for rentals or services)
    /// </summary>
    [Description("Day")]
    Day = 71,

    /// <summary>
    /// Month
    /// </summary>
    [Description("Month")]
    Month = 72,

    /// <summary>
    /// Year
    /// </summary>
    [Description("Year")]
    Year = 73,

    // Other Units
    /// <summary>
    /// Percentage (for services or calculations)
    /// </summary>
    [Description("Percent")]
    Percent = 80,

    /// <summary>
    /// Lot (batch or production lot)
    /// </summary>
    [Description("Lot")]
    Lot = 81,

    /// <summary>
    /// Sheet (for paper, metal, etc.)
    /// </summary>
    [Description("Sheet")]
    Sheet = 82
}
