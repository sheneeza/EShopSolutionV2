using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public class CategoryVariationService : ICategoryVariationService
{
    private readonly ICategoryVariationRepository _variationRepository;

    public CategoryVariationService(ICategoryVariationRepository variationRepository)
    {
        _variationRepository = variationRepository;
    }

    public Task<int> InsertAsync(CategoryVariation variation)
    {
        return _variationRepository.InsertAsync(variation);
    }

    public Task<int> UpdateAsync(CategoryVariation variation)
    {
        return _variationRepository.UpdateAsync(variation);
    }

    public Task<int> DeleteAsync(int id)
    {
        return _variationRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<CategoryVariation>> GetAllAsync()
    {
        return _variationRepository.GetAllAsync();
    }

    public Task<CategoryVariation?> GetByIdAsync(int id)
    {
        return _variationRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<CategoryVariation>> GetByCategoryIdAsync(int categoryId)
    {
        return _variationRepository.GetByCategoryIdAsync(categoryId);
    }
}