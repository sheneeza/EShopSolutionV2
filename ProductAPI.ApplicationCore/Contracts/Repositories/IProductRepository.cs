using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.ApplicationCore.Contracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<int> MarkInactiveAsync(int productId);
}