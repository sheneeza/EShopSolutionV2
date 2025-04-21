using Microsoft.AspNetCore.Mvc;
using ProductAPI.ApplicationCore.Contracts.Services;
using ProductAPI.ApplicationCore.Entities;

namespace ProductAPI.Controllers;

 [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private const string AllCategoriesCacheKey = "cache:allCategories";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        private readonly ICategoryService _categoryService;
        private readonly ICacheService    _cacheService;

        public CategoryController(
            ICategoryService categoryService,
            ICacheService    cacheService)
        {
            _categoryService = categoryService;
            _cacheService    = cacheService;
        }
        
        // POST: api/Category/SaveCategory
        [HttpPost("SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromBody] ProductCategory category)
        {
            if (category == null)
                return BadRequest("Category is null.");

            int result;
            if (category.Id == 0)
            {
                // Insert new category
                result = await _categoryService.InsertAsync(category);
            }
            else
            {
                // Update existing category
                result = await _categoryService.UpdateAsync(category);
            }

            if (result > 0)
            {
                // bust the cache so next GET will reload fresh data
                await _cacheService.RemoveAsync(AllCategoriesCacheKey);
                var msg = category.Id == 0 ? "Category created" : "Category updated";
                return Ok(new { Message = msg, Result = result, CategoryId = category.Id });
            }

            return BadRequest("Could not save the category.");
        }
        
        // GET: api/Category/GetAllCategory
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            // 1) Try read from cache
            var cached = await _cacheService.GetAsync<List<ProductCategory>>(AllCategoriesCacheKey);
            if (cached is not null && cached.Any())
                return Ok(cached);

            // 2) Fallback to DB
            var categories = (await _categoryService.GetAllAsync()).ToList();

            // 3) Store in cache
            await _cacheService.SetAsync(AllCategoriesCacheKey, categories, CacheDuration);

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
                return NotFound($"No category found with ID {id} to delete.");

            // bust the cache so next GET will reload fresh data
            await _cacheService.RemoveAsync(AllCategoriesCacheKey);
            return Ok(new { Message = "Category deleted", Result = result });
        }
    }