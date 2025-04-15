using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Repositories;

public interface ICategoryRepository : IRepository<ProductCategory>
{
    // Optional: extra methods specific to categories
    Task<IEnumerable<ProductCategory>> GetByParentCategoryIdAsync(int parentId);
}
