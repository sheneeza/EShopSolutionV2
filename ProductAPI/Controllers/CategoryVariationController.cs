using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class CategoryVariationController : ControllerBase
    {
        private readonly ICategoryVariationService _categoryVariationService;

        public CategoryVariationController(ICategoryVariationService categoryVariationService)
        {
            _categoryVariationService = categoryVariationService;
        }

        // POST: /api/CategoryVariation/Save
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] CategoryVariation variation)
        {
            if (variation == null)
            {
                return BadRequest("CategoryVariation is null.");
            }

            if (variation.Id == 0)
            {
                // Insert if Id == 0
                var result = await _categoryVariationService.InsertAsync(variation);
                return Ok(new { Message = "Variation created", RowsAffected = result, variation.Id });
            }
            else
            {
                // Otherwise update
                var result = await _categoryVariationService.UpdateAsync(variation);
                return Ok(new { Message = "Variation updated", RowsAffected = result });
            }
        }

        // GET: /api/CategoryVariation/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var variations = await _categoryVariationService.GetAllAsync();
            return Ok(variations);
        }

        // GET: /api/CategoryVariation/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var variation = await _categoryVariationService.GetByIdAsync(id);
            if (variation == null)
            {
                return NotFound($"No variation found with ID {id}.");
            }
            return Ok(variation);
        }

        // GET: /api/CategoryVariation/GetCategoryVariationByCategoryId/{categoryId}
        [HttpGet("GetCategoryVariationByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetCategoryVariationByCategoryId(int categoryId)
        {
            var variations = await _categoryVariationService.GetByCategoryIdAsync(categoryId);
            return Ok(variations);
        }

        // DELETE: /api/CategoryVariation/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryVariationService.DeleteAsync(id);
            if (result == 0)
            {
                return NotFound($"No variation found with ID {id} to delete.");
            }
            return Ok(new { Message = "Variation deleted", RowsAffected = result });
        }
    }