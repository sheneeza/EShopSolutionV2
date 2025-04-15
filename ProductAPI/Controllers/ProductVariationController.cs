using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class ProductVariationController : ControllerBase
    {
        private readonly IProductVariationService _productVariationService;

        public ProductVariationController(IProductVariationService productVariationService)
        {
            _productVariationService = productVariationService;
        }

        // POST: /api/ProductVariation/Save
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] ProductVariationValue productVariation)
        {
            if (productVariation == null)
                return BadRequest("ProductVariation is null.");

            if (productVariation.Id == 0)
            {
                // Insert
                var rows = await _productVariationService.InsertAsync(productVariation);
                return Ok(new { Message = "ProductVariation created", RowsAffected = rows, productVariation.Id });
            }
            else
            {
                // Update
                var rows = await _productVariationService.UpdateAsync(productVariation);
                return Ok(new { Message = "ProductVariation updated", RowsAffected = rows });
            }
        }

        // GET: /api/ProductVariation/GetProductVariation
        [HttpGet("GetProductVariation")]
        public async Task<IActionResult> GetProductVariation()
        {
            var variations = await _productVariationService.GetAllAsync();
            return Ok(variations);
        }
        
    }