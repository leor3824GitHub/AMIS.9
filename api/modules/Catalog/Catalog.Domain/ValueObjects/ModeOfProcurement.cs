namespace AMIS.WebApi.Catalog.Domain.ValueObjects;

/// <summary>
/// Mode of Procurement as per RA 9184 and related rules
/// </summary>
public enum ModeOfProcurement
{
    None = 0,
    PublicBidding = 1,
    LimitedSourceBidding = 2,
    DirectContracting = 3,
    RepeatOrder = 4,
    Shopping = 5,
    NegotiatedProcurement = 6,
    AgencyToAgency = 7,
    EmergencyProcurement = 8,
    SmallValueProcurement = 9
}
