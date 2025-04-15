using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Entities;
using ProductAPI.Infrastructure.Services;

namespace ProductAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        // GET: api/Product/GetAllProducts
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
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
        
        // POST: api/Product/Save
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");
            
            var result = await _productService.InsertAsync(product);
            return Ok(new { Message = "Product created", RowsAffected = result, product.Id });
        }
        
        // PUT: api/Product/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");
            
            if (id != product.Id)
                return BadRequest("Product ID mismatch.");
            
            var result = await _productService.UpdateAsync(product);
            return Ok(new { Message = "Product updated", RowsAffected = result });
        }
        
        
        [HttpPut("Inactive/{id}")]
        public async Task<IActionResult> Inactive(int id)
        {
            var rowsAffected = await _productService.MarkInactiveAsync(id);
            if (rowsAffected == 0)
                return NotFound($"No product found with ID {id} to mark inactive.");
            
            return Ok(new { Message = "Product marked inactive", RowsAffected = rowsAffected });
        }
        
        // DELETE: api/Product/DeleteProduct/{id}
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (result == 0)
                return NotFound($"No product found with ID {id} to delete.");
            return Ok(new { Message = "Product deleted", RowsAffected = result });
        }
    }