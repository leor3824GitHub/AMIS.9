namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Maps PPE types to RCA account codes
/// Implementation can be database-driven for flexibility
/// </summary>
public interface IPPEAccountCodeMapper
{
    /// <summary>
    /// Get RCA account code for a specific PPE type
    /// </summary>
    /// <param name="ppeType">PPE type (e.g., "Machinery", "ICT", "Transportation")</param>
    /// <returns>RCA account code</returns>
    string GetAccountCode(string ppeType);
}
