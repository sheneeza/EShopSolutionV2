using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories;

public class VariationValueRepository 
    : BaseRepository<VariationValue>, IVariationValueRepository
{
    public VariationValueRepository(ProductDbContext context) : base(context)
    {
    }

    // Example: Get VariationValues by VariationId
    public async Task<IEnumerable<VariationValue>> GetByVariationIdAsync(int variationId)
    {
        return await _dbSet
            .Where(v => v.VariationId == variationId)
            .ToListAsync();
    }
}