using AMIS.WebApi.Catalog.Domain;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Interfaces;

/// <summary>
/// Repository interface for SMIR aggregate root
/// </summary>
public interface ISuppliesAndMaterialsIssuanceReportRepository
{
    Task<SuppliesAndMaterialsIssuanceReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SuppliesAndMaterialsIssuanceReport?> GetBySmirNumberAsync(string smirNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SuppliesAndMaterialsIssuanceReport entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(SuppliesAndMaterialsIssuanceReport entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(SuppliesAndMaterialsIssuanceReport entity, CancellationToken cancellationToken = default);
}

/// <summary>
/// Read repository interface for SMIR queries
/// </summary>
public interface ISuppliesAndMaterialsIssuanceReportReadRepository
{
    Task<SuppliesAndMaterialsIssuanceReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SuppliesAndMaterialsIssuanceReport?> GetBySmirNumberAsync(string smirNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}
