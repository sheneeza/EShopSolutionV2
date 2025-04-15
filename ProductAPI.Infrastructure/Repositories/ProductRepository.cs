using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }
    
    public async Task<int> MarkInactiveAsync(int productId)
    {
        var product = await _dbSet.FindAsync(productId);
        if (product == null)
            return 0;
        if (!product.IsActive)
            return 0; 
        product.IsActive = false;
        return await _context.SaveChangesAsync();
    }
}