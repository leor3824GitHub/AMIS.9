using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Repositories;

internal sealed class PPETypeAccountMappingRepository : IPPETypeAccountMappingRepository
{
    private readonly CatalogDbContext _context;

    public PPETypeAccountMappingRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public IEnumerable<PPETypeAccountMapping> GetActiveMappings()
    {
        return _context.PPETypeAccountMappings
            .Where(m => m.IsActive && m.Deleted == null)
            .AsNoTracking()
            .ToList();
    }

    public async Task<PPETypeAccountMapping?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.PPETypeAccountMappings
            .FirstOrDefaultAsync(m => m.Id == id && m.Deleted == null, cancellationToken);
    }

    public async Task<PPETypeAccountMapping?> GetByTypeAsync(string ppeType, CancellationToken cancellationToken = default)
    {
        return await _context.PPETypeAccountMappings
            .FirstOrDefaultAsync(
                m => m.PPEType.ToUpper() == ppeType.ToUpper() && m.IsActive && m.Deleted == null,
                cancellationToken);
    }

    public async Task<List<PPETypeAccountMapping>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PPETypeAccountMappings
            .Where(m => m.Deleted == null)
            .OrderBy(m => m.PPEType)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default)
    {
        await _context.PPETypeAccountMappings.AddAsync(mapping, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PPETypeAccountMapping mapping, CancellationToken cancellationToken = default)
    {
        _context.PPETypeAccountMappings.Update(mapping);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
