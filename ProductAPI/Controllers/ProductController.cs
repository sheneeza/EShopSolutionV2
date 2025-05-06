using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Services;

namespace ProductAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICacheService   _cacheService;
        private const string AllProductsCacheKey = "cache:allProducts";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        
        public ProductController(IProductService productService, ICacheService   cacheService)
        {
            _productService = productService;
            _cacheService = cacheService;
        }
        
        // GET: api/Product/GetAllProducts
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            // 1) Try retrieving from Redis
            var cached = await _cacheService.GetAsync<List<Product>>(AllProductsCacheKey);
            if (cached != null && cached.Count > 0)
                return Ok(cached);

            // 2) Fallback to DB via service
            var productsEnumerable = await _productService.GetAllAsync();
            var products = productsEnumerable.ToList();

            // 3) Cache for next time
            await _cacheService.SetAsync(AllProductsCacheKey, products, CacheDuration);

            return Ok(products);
        }
        
        // GET: api/Product/GetProductById/{id}
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound($"No product found with ID {id}.");
            return Ok(product);
        }
        
        // GET: api/Product/GetProductByCategoryId/{categoryId}
        [HttpGet("GetProductByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetProductByCategoryId(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(products);
        }
        
        // GET: api/Product/GetProductByName/{name}
        [HttpGet("GetProductByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var products = await _productService.GetProductsByNameAsync(name);
            return Ok(products);
        }
        [Authorize(Roles = "Admin")]
        // POST: api/Product/Save
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");

            var rowsAffected = await _productService.InsertAsync(product);

            if (rowsAffected > 0)
            {
                // Invalidate cache
                await _cacheService.RemoveAsync(AllProductsCacheKey);
                return Ok(new
                {
                    Message       = "Product created",
                    RowsAffected  = rowsAffected,
                    ProductId     = product.Id
                });
            }

            return BadRequest("Could not create the product.");
        }
        
        // PUT: api/Product/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");

            if (id != product.Id)
                return BadRequest("Product ID mismatch.");

            var rowsAffected = await _productService.UpdateAsync(product);
            if (rowsAffected > 0)
            {
                // Invalidate cache
                await _cacheService.RemoveAsync(AllProductsCacheKey);
                return Ok(new
                {
                    Message      = "Product updated",
                    RowsAffected = rowsAffected
                });
            }

            return BadRequest("Could not update the product.");
        }
        
        
        // PUT: api/Product/Inactive/{id}
        [HttpPut("Inactive/{id}")]
        public async Task<IActionResult> Inactive(int id)
        {
            var rowsAffected = await _productService.MarkInactiveAsync(id);
            if (rowsAffected > 0)
            {
                // Invalidate cache
                await _cacheService.RemoveAsync(AllProductsCacheKey);
                return Ok(new
                {
                    Message      = "Product marked inactive",
                    RowsAffected = rowsAffected
                });
            }

            return NotFound($"No product found with ID {id} to mark inactive.");
        }

        
        // DELETE: api/Product/DeleteProduct/{id}
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var rowsAffected = await _productService.DeleteAsync(id);
            if (rowsAffected > 0)
            {
                // Invalidate cache
                await _cacheService.RemoveAsync(AllProductsCacheKey);
                return Ok(new
                {
                    Message      = "Product deleted",
                    RowsAffected = rowsAffected
                });
            }

            return NotFound($"No product found with ID {id} to delete.");
        }
    }