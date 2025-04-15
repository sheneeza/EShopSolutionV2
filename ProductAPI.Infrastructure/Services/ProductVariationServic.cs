using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public class ProductVariationService : IProductVariationService
{
    private readonly IProductVariationRepository _repository;

    public ProductVariationService(IProductVariationRepository repository)
    {
        _repository = repository;
    }

    public Task<int> InsertAsync(ProductVariationValue productVariation)
    {
        return _repository.InsertAsync(productVariation);
    }

    public Task<int> UpdateAsync(ProductVariationValue productVariation)
    {
        return _repository.UpdateAsync(productVariation);
    }

    public Task<int> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    public Task<IEnumerable<ProductVariationValue>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<ProductVariationValue?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<IEnumerable<ProductVariationValue>> GetByProductIdAsync(int productId)
    {
        return _repository.GetByProductIdAsync(productId);
    }

    public Task<IEnumerable<ProductVariationValue>> GetByVariationValueIdAsync(int variationValueId)
    {
        return _repository.GetByVariationValueIdAsync(variationValueId);
    }
}