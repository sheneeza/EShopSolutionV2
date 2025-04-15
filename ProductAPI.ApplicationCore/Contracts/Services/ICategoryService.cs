using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Services;

public interface ICategoryService
{
    Task<int> InsertAsync(ProductCategory category);
    Task<int> UpdateAsync(ProductCategory category);
    Task<int> DeleteAsync(int id);
    Task<IEnumerable<ProductCategory>> GetAllAsync();
    Task<ProductCategory?> GetByIdAsync(int id);
    Task<IEnumerable<ProductCategory>> GetByParentCategoryIdAsync(int parentId);
}