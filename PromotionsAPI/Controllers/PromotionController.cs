using Microsoft.AspNetCore.Mvc;
using PromotionsAPI.ApplicationCore.Contracts.Services;
using PromotionsAPI.ApplicationCore.Entities;
using PromotionsAPI.ApplicationCore.Models;

namespace PromotionsAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // GET: api/Promotion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetAllPromotions()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        // GET: api/Promotion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Promotion>> GetPromotionById(int id)
        {
            var promotion = await _promotionService.GetPromotionWithDetailsByIdAsync(id);
            if (promotion == null)
                return NotFound($"Promotion with ID {id} not found.");
            return Ok(promotion);
        }

        // GET: api/Promotion/active
        [HttpGet("activePromotions")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetActivePromotions()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return Ok(promotions);
        }
        
        [HttpGet("promotionByProductName")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotionsByProductName([FromQuery] string productName)
        {
            var promotions = await _promotionService.GetPromotionsByProductNameAsync(productName);
            return Ok(promotions);
        }


        // POST: api/Promotion
        [HttpPost]
        public async Task<ActionResult> CreatePromotion([FromBody] PromotionRequestModel model)
        {
            // Map the request model to the Promotion entity
            var promotion = new Promotion
            {
                Name = model.Name,
                Description = model.Description,
                Discount = model.Discount,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PromotionDetails = model.PromotionDetails?.Select(pd => new PromotionDetail
                {
                    ProductCategoryId = pd.ProductCategoryId,
                    ProductCategoryName = pd.ProductCategoryName
                }).ToList() ?? new List<PromotionDetail>()
            };

            var result = await _promotionService.CreatePromotionAsync(promotion);
            if (result > 0)
                return Ok("Promotion created successfully.");
            return BadRequest("Failed to create the promotion.");
        }

        // PUT: api/Promotion
        [HttpPut]
        public async Task<ActionResult> UpdatePromotion([FromBody] PromotionRequestModel model)
        {
            var existing = await _promotionService.GetPromotionByIdAsync(model.Id);
            if (existing == null)
                return NotFound($"Promotion with ID {model.Id} not found.");

            // Update the properties
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.Discount = model.Discount;
            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;
            if (model.PromotionDetails != null)
            {
                existing.PromotionDetails = model.PromotionDetails.Select(pd => new PromotionDetail
                {
                    ProductCategoryId = pd.ProductCategoryId,
                    ProductCategoryName = pd.ProductCategoryName
                }).ToList();
            }

            var result = await _promotionService.UpdatePromotionAsync(existing);
            if (result > 0)
                return Ok("Promotion updated successfully.");
            return BadRequest("Failed to update the promotion.");
        }

        // DELETE: api/Promotion/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePromotion(int id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            if (result > 0)
                return Ok($"Promotion with ID {id} deleted successfully.");
            return NotFound($"Promotion with ID {id} not found.");
        }
    }