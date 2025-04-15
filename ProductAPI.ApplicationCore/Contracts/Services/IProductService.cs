using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public interface IProductService
{
    Task<int> InsertAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<int> DeleteAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    
    Task<int> MarkInactiveAsync(int productId);
}