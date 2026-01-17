using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Repositories;

public class RcaAccountCodeRepository : IRcaAccountCodeRepository
{
    private readonly InventoriesDbContext _context;

    public RcaAccountCodeRepository(InventoriesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RcaAccountCodeDefinition>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RcaAccountCodes
            .Where(a => a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<RcaAccountCodeDefinition?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.RcaAccountCodes
            .FirstOrDefaultAsync(a => a.IsActive && a.Key == key, cancellationToken);
    }

    public async Task AddAsync(RcaAccountCodeDefinition definition, CancellationToken cancellationToken = default)
    {
        _context.RcaAccountCodes.Add(definition);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RcaAccountCodeDefinition definition, CancellationToken cancellationToken = default)
    {
        _context.RcaAccountCodes.Update(definition);
        await _context.SaveChangesAsync(cancellationToken);
    }
}


