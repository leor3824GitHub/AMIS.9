namespace AMIS.WebApi.Inventories.Application.PropertyCodes;

public sealed class CoaPropertyCodeOptions
{
    public string AgencyCode { get; set; } = "NFA";
    public bool ResetSequenceAnnually { get; set; } = true;
    public int SequenceLength { get; set; } = 4;
    public string DefaultOfficeCode { get; set; } = "0000";
    public string DefaultClassCode { get; set; } = "00";
    public string DefaultCategoryCode { get; set; } = "00";
    public string DefaultItemCode { get; set; } = "000";
}
