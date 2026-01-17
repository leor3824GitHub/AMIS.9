using AMIS.Modules.Catalog.Application.MaterialsIssuance.Interfaces;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace AMIS.Modules.Catalog.Infrastructure.Persistence.MaterialsIssuance;

public class SuppliesAndMaterialsIssuanceReportRepository
    : ISuppliesAndMaterialsIssuanceReportRepository, ISuppliesAndMaterialsIssuanceReportReadRepository
{
    private readonly DbContext _context;

    public SuppliesAndMaterialsIssuanceReportRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<SuppliesAndMaterialsIssuanceReport?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<SuppliesAndMaterialsIssuanceReport>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<SuppliesAndMaterialsIssuanceReport?> GetBySmirNumberAsync(
        string smirNumber,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<SuppliesAndMaterialsIssuanceReport>()
            .FirstOrDefaultAsync(x => x.SmirNumber == smirNumber, cancellationToken);
    }

    public async Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<SuppliesAndMaterialsIssuanceReport>()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SuppliesAndMaterialsIssuanceReport>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<SuppliesAndMaterialsIssuanceReport>()
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        SuppliesAndMaterialsIssuanceReport entity,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<SuppliesAndMaterialsIssuanceReport>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        SuppliesAndMaterialsIssuanceReport entity,
        CancellationToken cancellationToken = default)
    {
        _context.Set<SuppliesAndMaterialsIssuanceReport>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        SuppliesAndMaterialsIssuanceReport entity,
        CancellationToken cancellationToken = default)
    {
        _context.Set<SuppliesAndMaterialsIssuanceReport>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
