using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromotionsAPI.ApplicationCore.Contracts.Services;
using PromotionsAPI.ApplicationCore.Entities;
using PromotionsAPI.ApplicationCore.Models;
using PromotionsAPI.Utility;

namespace PromotionsAPI.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly PromotionEventPublisher  _publisher;

        public PromotionController(IPromotionService promotionService, PromotionEventPublisher publisher)
        {
            _promotionService = promotionService;
            _publisher = publisher;
        }

        // GET: api/Promotion
        [HttpGet]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetAllPromotions()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        // GET: api/Promotion/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<Promotion>> GetPromotionById(int id)
        {
            var promotion = await _promotionService.GetPromotionWithDetailsByIdAsync(id);
            if (promotion == null)
                return NotFound($"Promotion with ID {id} not found.");
            return Ok(promotion);
        }

        // GET: api/Promotion/active
        [HttpGet("activePromotions")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetActivePromotions()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return Ok(promotions);
        }
        
        [HttpGet("promotionByProductName")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotionsByProductName([FromQuery] string productName)
        {
            var promotions = await _promotionService.GetPromotionsByProductNameAsync(productName);
            return Ok(promotions);
        }


        // POST: api/Promotion
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreatePromotion([FromBody] PromotionRequestModel model)
        {
            // 1) Map & save
            var promotion = new Promotion
            {
                Name             = model.Name,
                Description      = model.Description,
                Discount         = model.Discount,
                StartDate        = model.StartDate,
                EndDate          = model.EndDate,
                PromotionDetails = model.PromotionDetails?
                                       .Select(pd => new PromotionDetail {
                                           ProductCategoryId   = pd.ProductCategoryId,
                                           ProductCategoryName = pd.ProductCategoryName
                                       }).ToList()
                                   ?? new List<PromotionDetail>()
            };

            var newId = await _promotionService.CreatePromotionAsync(promotion);
            if (newId <= 0)
                return BadRequest("Failed to create the promotion.");

            // 2) Publish started event
            var startedEvt = new PromotionStartedEvent(
                promotion.Id,
                promotion.Name,
                promotion.Description,
                promotion.Discount,
                promotion.StartDate,
                promotion.EndDate
            );
            await _publisher.PublishPromotionStartedAsync(startedEvt);

            return Ok("Promotion created and PromotionStarted event published.");
        }

        // PUT: api/Promotion
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePromotion([FromBody] PromotionRequestModel model)
        {
            var existing = await _promotionService.GetPromotionByIdAsync(model.Id);
            if (existing == null)
                return NotFound($"Promotion with ID {model.Id} not found.");

            // remember old end date
            var oldEnd = existing.EndDate;

            // update properties
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.Discount = model.Discount;
            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;
            if (model.PromotionDetails != null)
            {
                existing.PromotionDetails = model.PromotionDetails
                    .Select(pd => new PromotionDetail
                    {
                        ProductCategoryId = pd.ProductCategoryId,
                        ProductCategoryName = pd.ProductCategoryName
                    })
                    .ToList();
            }

            var updated = await _promotionService.UpdatePromotionAsync(existing);
            if (updated <= 0)
                return BadRequest("Failed to update the promotion.");

            // If EndDate is now in the past, publish ended event
            if (oldEnd < existing.EndDate && existing.EndDate <= DateTime.UtcNow)
            {
                var endedEvt = new PromotionEndedEvent(
                    existing.Id,
                    existing.Name,
                    existing.EndDate
                );
                await _publisher.PublishPromotionEndedAsync(endedEvt);
            }

            return Ok("Promotion updated and any PromotionEnded event published.");
        
    }

        // DELETE: api/Promotion/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePromotion(int id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            if (result > 0)
                return Ok($"Promotion with ID {id} deleted successfully.");
            return NotFound($"Promotion with ID {id} not found.");
        }
    }