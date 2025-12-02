using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Database-driven PPE account code mapper
/// Allows runtime configuration of PPE type mappings
/// Falls back to default mapper if database is unavailable
/// </summary>
public class DatabasePPEAccountCodeMapper : IPPEAccountCodeMapper
{
    private readonly IPPETypeAccountMappingRepository _repository;
    private readonly IPPEAccountCodeMapper _fallbackMapper;
    private Dictionary<string, string>? _cachedMappings;
    private DateTime _lastCacheTime = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public DatabasePPEAccountCodeMapper(
        IPPETypeAccountMappingRepository repository,
        DefaultPPEAccountCodeMapper fallbackMapper)
    {
        _repository = repository;
        _fallbackMapper = fallbackMapper;
    }

    public string GetAccountCode(string ppeType)
    {
        if (string.IsNullOrWhiteSpace(ppeType))
            return RCAAccountCode.OtherPropertyPlantAndEquipment;

        try
        {
            var mappings = GetMappings();
            return mappings.TryGetValue(ppeType.ToUpperInvariant(), out var accountCode)
                ? accountCode
                : RCAAccountCode.OtherPropertyPlantAndEquipment;
        }
        catch
        {
            // Fallback to default mapper if database access fails
            return _fallbackMapper.GetAccountCode(ppeType);
        }
    }

    private Dictionary<string, string> GetMappings()
    {
        // Return cached mappings if still valid
        if (_cachedMappings != null && DateTime.UtcNow - _lastCacheTime < CacheDuration)
            return _cachedMappings;

        // Refresh cache from database
        var mappings = _repository.GetActiveMappings();
        _cachedMappings = mappings.ToDictionary(
            m => m.PPEType.ToUpperInvariant(),
            m => m.RCAAccountCode,
            StringComparer.OrdinalIgnoreCase);
        _lastCacheTime = DateTime.UtcNow;

        return _cachedMappings;
    }
}

/// <summary>
/// Repository interface for PPE type account mappings
/// </summary>
public interface IPPETypeAccountMappingRepository
{
    IEnumerable<PPETypeAccountMapping> GetActiveMappings();
    Task<PPETypeAccountMapping?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PPETypeAccountMapping?> GetByTypeAsync(string ppeType, CancellationToken cancellationToken = default);
    Task<List<PPETypeAccountMapping>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default);
    Task UpdateAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default);
}
