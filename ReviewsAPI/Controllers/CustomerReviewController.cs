using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewsAPI.ApplicationCore.Contracts.Services;
using ReviewsAPI.ApplicationCore.Entities;
using ReviewsAPI.ApplicationCore.Models;

namespace ReviewsAPI.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerReviewController : ControllerBase
    {
        private readonly ICustomerReviewService _service;

        public CustomerReviewController(ICustomerReviewService service)
        {
            _service = service;
        }

        // GET /api/CustomerReview
        [HttpGet]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviews()
        {
            var reviews = await _service.GetAllAsync();
            return Ok(reviews);
        }

        // POST /api/CustomerReview
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateReview([FromBody] CustomerReviewRequestModel model)
        {
            var newReview = new Review
            {
                CustomerId = model.CustomerId,
                CustomerName = model.CustomerName,
                OrderId = model.OrderId,
                OrderDate = model.OrderDate,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                RatingValue = model.RatingValue,
                Comment = model.Comment,
                ReviewDate = model.ReviewDate,
                Status = "Pending"
            };

            var result = await _service.CreateAsync(newReview);
            if (result > 0)
                return Ok("Review created successfully.");
            else
                return BadRequest("Failed to create review.");
        }

        // PUT /api/CustomerReview
        [HttpPut]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> UpdateReview([FromBody] CustomerReviewRequestModel model)
        {
            var existing = await _service.GetByIdAsync(model.Id);
            if (existing == null)
                return NotFound($"Review with ID {model.Id} not found.");

            // Update fields
            existing.CustomerId = model.CustomerId;
            existing.CustomerName = model.CustomerName;
            existing.OrderId = model.OrderId;
            existing.OrderDate = model.OrderDate;
            existing.ProductId = model.ProductId;
            existing.ProductName = model.ProductName;
            existing.RatingValue = model.RatingValue;
            existing.Comment = model.Comment;
            existing.ReviewDate = model.ReviewDate;
            // Keep existing.Status as is (or reset to "Pending" if needed)

            var result = await _service.UpdateAsync(existing);
            if (result > 0)
                return Ok("Review updated successfully.");
            else
                return BadRequest("Failed to update review.");
        }

        // DELETE /api/CustomerReview/delete/{id}
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0)
                return Ok($"Review with ID {id} deleted successfully.");
            else
                return NotFound($"Review with ID {id} not found.");
        }

        // GET /api/CustomerReview/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUser(int userId)
        {
            var reviews = await _service.GetReviewsByUserAsync(userId);
            return Ok(reviews);
        }

        // GET /api/CustomerReview/product/{productId}
        [HttpGet("product/{productId}")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByProduct(int productId)
        {
            var reviews = await _service.GetReviewsByProductAsync(productId);
            return Ok(reviews);
        }

        // PUT /api/CustomerReview/approve/{id}
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ApproveReview(int id)
        {
            var success = await _service.ApproveReviewAsync(id);
            if (!success)
                return NotFound($"Review with ID {id} not found.");

            return Ok("Review approved successfully.");
        }

        // PUT /api/CustomerReview/reject/{id}
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RejectReview(int id)
        {
            var success = await _service.RejectReviewAsync(id);
            if (!success)
                return NotFound($"Review with ID {id} not found.");

            return Ok("Review rejected successfully.");
        }
    }