using Microsoft.EntityFrameworkCore;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Data;

namespace ProductAPI.Infrastructure.Repositories;

public class ProductVariationRepository 
    : BaseRepository<ProductVariationValue>, IProductVariationRepository
{
    public ProductVariationRepository(ProductDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductVariationValue>> GetByProductIdAsync(int productId)
    {
        return await _dbSet
            .Where(pv => pv.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductVariationValue>> GetByVariationValueIdAsync(int variationValueId)
    {
        return await _dbSet
            .Where(pv => pv.VariationValueId == variationValueId)
            .ToListAsync();
    }
}