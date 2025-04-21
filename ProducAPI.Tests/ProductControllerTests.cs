using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Controllers;
using ProductAPI.Infrastructure.Services;

namespace ProductServiceTests;

public class ProductControllerTests
    {
        private const string CacheKey = "cache:allProducts";

        private readonly Mock<IProductService> _productService;
        private readonly Mock<ICacheService>   _cacheService;
        private readonly ProductController    _ctrl;

        public ProductControllerTests()
        {
            _productService = new Mock<IProductService>();
            _cacheService   = new Mock<ICacheService>();
            _ctrl           = new ProductController(_productService.Object, _cacheService.Object);
        }

        [Fact]
        public async Task GetAllProducts_CacheHit_ReturnsCachedList()
        {
            var cached = new List<Product> { new() { Id = 1 }, new() { Id = 2 } };
            _cacheService
                .Setup(c => c.GetAsync<List<Product>>(CacheKey))
                .ReturnsAsync(cached);

            var result = await _ctrl.GetAllProducts();
            var ok     = Assert.IsType<OkObjectResult>(result);
            var list   = Assert.IsAssignableFrom<List<Product>>(ok.Value);

            Assert.Equal(2, list.Count);
            _productService.Verify(s => s.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task GetAllProducts_CacheMiss_FallsBackToService_And_Caches()
        {
            _cacheService
                .Setup(c => c.GetAsync<List<Product>>(CacheKey))
                .ReturnsAsync((List<Product>?)null);

            var svcList = new[] { new Product { Id = 3 } };
            _productService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(svcList);

            var result = await _ctrl.GetAllProducts();
            var ok     = Assert.IsType<OkObjectResult>(result);
            var list   = Assert.IsAssignableFrom<List<Product>>(ok.Value);

            Assert.Single(list);
            _cacheService.Verify(c => c.SetAsync(CacheKey, list, TimeSpan.FromMinutes(5)), Times.Once);
        }

        [Fact]
        public async Task GetProductById_Found_ReturnsOk()
        {
            var p = new Product { Id = 10 };
            _productService.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(p);

            var result = await _ctrl.GetProductById(10);
            var ok     = Assert.IsType<OkObjectResult>(result);
            Assert.Same(p, ok.Value);
        }

        [Fact]
        public async Task GetProductById_NotFound_Returns404()
        {
            _productService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            var result = await _ctrl.GetProductById(99);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No product found", nf.Value.ToString());
        }

        [Fact]
        public async Task GetProductByCategoryId_ReturnsOkList()
        {
            var list = new[] { new Product { Id = 5 } };
            _productService.Setup(s => s.GetProductsByCategoryIdAsync(2)).ReturnsAsync(list);

            var result = await _ctrl.GetProductByCategoryId(2);
            var ok     = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Product>>(ok.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task Save_NullProduct_ReturnsBadRequest()
        {
            var result = await _ctrl.Save(null!);
            var bad    = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product is null.", bad.Value);
        }

        [Fact]
        public async Task Save_OnSuccess_InvalidatesCache_ReturnsOk()
        {
            var prod = new Product { Id = 7 };
            _productService.Setup(s => s.InsertAsync(prod)).ReturnsAsync(1);

            var result = await _ctrl.Save(prod);
            var ok     = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Product created", ok.Value.ToString());
            _cacheService.Verify(c => c.RemoveAsync(CacheKey), Times.Once);
        }

        [Fact]
        public async Task Save_OnFailure_ReturnsBadRequest()
        {
            var prod = new Product();
            _productService.Setup(s => s.InsertAsync(prod)).ReturnsAsync(0);

            var result = await _ctrl.Save(prod);
            var bad    = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Could not create the product.", bad.Value);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var prod = new Product { Id = 3 };
            var result = await _ctrl.Update(4, prod);
            var bad    = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product ID mismatch.", bad.Value);
        }

        [Fact]
        public async Task Update_OnSuccess_InvalidatesCache_ReturnsOk()
        {
            var prod = new Product { Id = 9 };
            _productService.Setup(s => s.UpdateAsync(prod)).ReturnsAsync(1);

            var result = await _ctrl.Update(9, prod);
            var ok     = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Product updated", ok.Value.ToString());
            _cacheService.Verify(c => c.RemoveAsync(CacheKey), Times.Once);
        }

        [Fact]
        public async Task Inactive_OnSuccess_InvalidatesCache_ReturnsOk()
        {
            _productService.Setup(s => s.MarkInactiveAsync(8)).ReturnsAsync(1);

            var result = await _ctrl.Inactive(8);
            var ok     = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Product marked inactive", ok.Value.ToString());
            _cacheService.Verify(c => c.RemoveAsync(CacheKey), Times.Once);
        }

        [Fact]
        public async Task Inactive_NotFound_Returns404()
        {
            _productService.Setup(s => s.MarkInactiveAsync(It.IsAny<int>())).ReturnsAsync(0);

            var result = await _ctrl.Inactive(123);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No product found", nf.Value.ToString());
        }

        [Fact]
        public async Task DeleteProduct_OnSuccess_InvalidatesCache_ReturnsOk()
        {
            _productService.Setup(s => s.DeleteAsync(6)).ReturnsAsync(1);

            var result = await _ctrl.DeleteProduct(6);
            var ok     = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Product deleted", ok.Value.ToString());
            _cacheService.Verify(c => c.RemoveAsync(CacheKey), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_NotFound_Returns404()
        {
            _productService.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(0);

            var result = await _ctrl.DeleteProduct(999);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("No product found", nf.Value.ToString());
        }
    }