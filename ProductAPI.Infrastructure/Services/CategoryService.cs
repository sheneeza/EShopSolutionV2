using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
        
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
        
    public Task<int> InsertAsync(ProductCategory category)
    {
        return _categoryRepository.InsertAsync(category);
    }
        
    public Task<int> UpdateAsync(ProductCategory category)
    {
        return _categoryRepository.UpdateAsync(category);
    }
        
    public Task<int> DeleteAsync(int id)
    {
        return _categoryRepository.DeleteAsync(id);
    }
        
    public Task<IEnumerable<ProductCategory>> GetAllAsync()
    {
        return _categoryRepository.GetAllAsync();
    }
        
    public Task<ProductCategory?> GetByIdAsync(int id)
    {
        return _categoryRepository.GetByIdAsync(id);
    }
        
    public Task<IEnumerable<ProductCategory>> GetByParentCategoryIdAsync(int parentId)
    {
        return _categoryRepository.GetByParentCategoryIdAsync(parentId);
    }
}