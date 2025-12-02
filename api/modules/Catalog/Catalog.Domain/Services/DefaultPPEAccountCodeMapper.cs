using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Default in-memory implementation of PPE account code mapping
/// For production, consider database-driven implementation for configurability
/// </summary>
public class DefaultPPEAccountCodeMapper : IPPEAccountCodeMapper
{
    private static readonly Dictionary<string, string> Mappings = new(StringComparer.OrdinalIgnoreCase)
    {
        // Machinery & Equipment
        ["MACHINERY"] = RCAAccountCode.MachineryAndEquipment,
        ["EQUIPMENT"] = RCAAccountCode.MachineryAndEquipment,
        
        // Transportation Equipment
        ["TRANSPORTATION"] = RCAAccountCode.TransportationEquipment,
        ["VEHICLE"] = RCAAccountCode.TransportationEquipment,
        ["AUTOMOTIVE"] = RCAAccountCode.TransportationEquipment,
        
        // Furniture & Fixtures
        ["FURNITURE"] = RCAAccountCode.FurnitureFixturesAndBooksEquipment,
        ["FIXTURES"] = RCAAccountCode.FurnitureFixturesAndBooksEquipment,
        
        // ICT Equipment
        ["ICT"] = RCAAccountCode.ICTEquipment,
        ["COMPUTER"] = RCAAccountCode.ICTEquipment,
        ["IT"] = RCAAccountCode.ICTEquipment,
        ["TECHNOLOGY"] = RCAAccountCode.ICTEquipment
    };

    public string GetAccountCode(string ppeType)
    {
        if (string.IsNullOrWhiteSpace(ppeType))
            return RCAAccountCode.OtherPropertyPlantAndEquipment;

        return Mappings.TryGetValue(ppeType, out var accountCode) 
            ? accountCode 
            : RCAAccountCode.OtherPropertyPlantAndEquipment;
    }
}
