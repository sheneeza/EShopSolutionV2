using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<ProductCategory>, ICategoryRepository
{
    public CategoryRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductCategory>> GetByParentCategoryIdAsync(int parentId)
    {
        return await _dbSet.Where(c => c.ParentCategoryId == parentId).ToListAsync();
    }
}