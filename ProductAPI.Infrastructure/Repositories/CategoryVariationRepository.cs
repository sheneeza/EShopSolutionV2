using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories;

public class CategoryVariationRepository : BaseRepository<CategoryVariation>, ICategoryVariationRepository
{
    public CategoryVariationRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CategoryVariation>> GetByCategoryIdAsync(int categoryId)
    {
        return await _dbSet
            .Where(v => v.CategoryId == categoryId)
            .ToListAsync();
    }
}