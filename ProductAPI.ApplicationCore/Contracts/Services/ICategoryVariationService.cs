using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Services;

public interface ICategoryVariationService
{
    Task<int> InsertAsync(CategoryVariation variation);
    Task<int> UpdateAsync(CategoryVariation variation);
    Task<int> DeleteAsync(int id);
    Task<IEnumerable<CategoryVariation>> GetAllAsync();
    Task<CategoryVariation?> GetByIdAsync(int id);
    Task<IEnumerable<CategoryVariation>> GetByCategoryIdAsync(int categoryId);
}