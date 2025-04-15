using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Repositories;

public interface IProductVariationRepository : IRepository<ProductVariationValue>
{
    Task<IEnumerable<ProductVariationValue>> GetByProductIdAsync(int productId);
    
    Task<IEnumerable<ProductVariationValue>> GetByVariationValueIdAsync(int variationValueId);
}

