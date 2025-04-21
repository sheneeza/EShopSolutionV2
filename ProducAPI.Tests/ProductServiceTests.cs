using Moq;
using ProductAPI.ApplicationCore.Contracts.Repositories;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Services;

namespace ProductServiceTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repo;
    private readonly ProductService _svc;

    public ProductServiceTests()
    {
        _repo = new Mock<IProductRepository>();
        _svc  = new ProductService(_repo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        // Arrange
        var list = new[] {
            new Product { Id = 1, Name = "A" },
            new Product { Id = 2, Name = "B" }
        };
        _repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(list);

        // Act
        var result = (await _svc.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "A");
        _repo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_KnownId_ReturnsProduct()
    {
        // Arrange
        var p = new Product { Id = 5, Name = "Test" };
        _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(p);

        // Act
        var result = await _svc.GetByIdAsync(5);

        // Assert
        Assert.Same(p, result);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _svc.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }
}