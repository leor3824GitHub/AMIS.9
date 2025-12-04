namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Repository interface for PPE Type Account Mappings
/// </summary>
public interface IPPETypeAccountMappingRepository
{
    /// <summary>
    /// Get all active PPE type mappings
    /// </summary>
    IEnumerable<PPETypeAccountMapping> GetActiveMappings();

    /// <summary>
    /// Get mapping by ID
    /// </summary>
    Task<PPETypeAccountMapping?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get mapping by PPE type
    /// </summary>
    Task<PPETypeAccountMapping?> GetByTypeAsync(string ppeType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all mappings including inactive
    /// </summary>
    Task<List<PPETypeAccountMapping>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new mapping
    /// </summary>
    Task AddAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing mapping
    /// </summary>
    Task UpdateAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default);
}
