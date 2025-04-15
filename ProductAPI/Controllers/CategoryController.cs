using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Controllers;

 [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        // POST: api/Category/SaveCategory
        [HttpPost("SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody] ProductCategory category)
        {
            if (category == null)
            {
                return BadRequest("Category is null.");
            }
            
            if (category.Id == 0)
            {
                // Insert new category
                var result = await _categoryService.InsertAsync(category);
                return Ok(new { Message = "Category created", Result = result, CategoryId = category.Id });
            }
            else
            {
                // Update existing category
                var result = await _categoryService.UpdateAsync(category);
                return Ok(new { Message = "Category updated", Result = result });
            }
        }
        
        // GET: api/Category/GetAllCategory
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        
        // GET: api/Category/GetCategoryById/{id}
        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound($"No category found with ID {id}.");
            }
            return Ok(category);
        }
        
        // GET: api/Category/GetCategoryByParentCategoryId/{parentId}
        [HttpGet("GetCategoryByParentCategoryId/{parentId}")]
        public async Task<IActionResult> GetCategoryByParentCategoryId(int parentId)
        {
            var categories = await _categoryService.GetByParentCategoryIdAsync(parentId);
            return Ok(categories);
        }
        
        // DELETE: api/Category/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result == 0)
            {
                return NotFound($"No category found with ID {id} to delete.");
            }
            return Ok(new { Message = "Category deleted", Result = result });
        }
    }