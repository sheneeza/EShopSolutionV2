using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
        
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
        
    public Task<int> InsertAsync(Product product)
    {
        return _productRepository.InsertAsync(product);
    }
        
    public Task<int> UpdateAsync(Product product)
    {
        return _productRepository.UpdateAsync(product);
    }
        
    public Task<int> DeleteAsync(int id)
    {
        return _productRepository.DeleteAsync(id);
    }
        
    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return _productRepository.GetAllAsync();
    }
        
    public Task<Product?> GetByIdAsync(int id)
    {
        return _productRepository.GetByIdAsync(id);
    }
        
    public Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return _productRepository.GetProductsByCategoryIdAsync(categoryId);
    }
        
    public Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        return _productRepository.GetProductsByNameAsync(name);
    }
    
    public Task<int> MarkInactiveAsync(int productId)
    {
        return _productRepository.MarkInactiveAsync(productId);
    }
}