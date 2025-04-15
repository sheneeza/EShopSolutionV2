using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Services;

public interface IProductVariationService
{
    Task<int> InsertAsync(ProductVariationValue productVariation);
    Task<int> UpdateAsync(ProductVariationValue productVariation);
    Task<int> DeleteAsync(int id);
        
    Task<IEnumerable<ProductVariationValue>> GetAllAsync();
    Task<ProductVariationValue?> GetByIdAsync(int id);

    Task<IEnumerable<ProductVariationValue>> GetByProductIdAsync(int productId);
    Task<IEnumerable<ProductVariationValue>> GetByVariationValueIdAsync(int variationValueId);
}
