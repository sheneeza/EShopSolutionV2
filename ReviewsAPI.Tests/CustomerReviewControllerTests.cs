using Microsoft.AspNetCore.Mvc;
using Moq;
using ReviewsAPI.ApplicationCore.Contracts.Services;
using ReviewsAPI.ApplicationCore.Entities;
using ReviewsAPI.ApplicationCore.Models;
using ReviewsAPI.Controllers;

namespace ReviewsAPI.Tests;

public class CustomerReviewControllerTests
    {
        private readonly Mock<ICustomerReviewService> _service;
        private readonly CustomerReviewController    _ctrl;

        public CustomerReviewControllerTests()
        {
            _service = new Mock<ICustomerReviewService>();
            _ctrl    = new CustomerReviewController(_service.Object);
        }

        [Fact]
        public async Task GetAllReviews_ReturnsOk_WithList()
        {
            var list = new[]
            {
                new Review { Id = 1 },
                new Review { Id = 2 }
            };
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

            var action = await _ctrl.GetAllReviews();
            var ok     = Assert.IsType<OkObjectResult>(action.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Review>>(ok.Value);

            Assert.Equal(2, returned.Count());
            _service.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateReview_OnSuccess_ReturnsOk()
        {
            var model = new CustomerReviewRequestModel { CustomerId = 1, ProductId = 2 };
            _service.Setup(s => s.CreateAsync(It.IsAny<Review>())).ReturnsAsync(5);

            var result = await _ctrl.CreateReview(model);
            var ok     = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Review created successfully.", ok.Value);
        }

        [Fact]
        public async Task CreateReview_OnFailure_ReturnsBadRequest()
        {
            _service.Setup(s => s.CreateAsync(It.IsAny<Review>())).ReturnsAsync(0);

            var result = await _ctrl.CreateReview(new CustomerReviewRequestModel());
            var bad    = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Failed to create review.", bad.Value);
        }

        [Fact]
        public async Task UpdateReview_NotFound_ReturnsNotFound()
        {
            var model = new CustomerReviewRequestModel { Id = 10 };
            _service.Setup(s => s.GetByIdAsync(10)).ReturnsAsync((Review?)null);

            var result = await _ctrl.UpdateReview(model);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Review with ID 10 not found.", nf.Value);
        }

        [Fact]
        public async Task UpdateReview_OnSuccess_ReturnsOk()
        {
            var existing = new Review { Id = 3, Status = "Pending" };
            _service.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(existing);
            _service.Setup(s => s.UpdateAsync(existing)).ReturnsAsync(1);

            var model = new CustomerReviewRequestModel { Id = 3 };
            var result = await _ctrl.UpdateReview(model);
            var ok     = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Review updated successfully.", ok.Value);
        }

        [Fact]
        public async Task UpdateReview_OnFailure_ReturnsBadRequest()
        {
            var existing = new Review { Id = 4 };
            _service.Setup(s => s.GetByIdAsync(4)).ReturnsAsync(existing);
            _service.Setup(s => s.UpdateAsync(existing)).ReturnsAsync(0);

            var model = new CustomerReviewRequestModel { Id = 4 };
            var result = await _ctrl.UpdateReview(model);
            var bad    = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Failed to update review.", bad.Value);
        }

        [Fact]
        public async Task DeleteReview_OnSuccess_ReturnsOk()
        {
            _service.Setup(s => s.DeleteAsync(2)).ReturnsAsync(1);

            var result = await _ctrl.DeleteReview(2);
            var ok     = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Review with ID 2 deleted successfully.", ok.Value);
        }

        [Fact]
        public async Task DeleteReview_NotFound_ReturnsNotFound()
        {
            _service.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(0);

            var result = await _ctrl.DeleteReview(99);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Review with ID 99 not found.", nf.Value);
        }

        [Fact]
        public async Task GetReviewsByUser_ReturnsOk_WithList()
        {
            var list = new[] { new Review { Id = 5, CustomerId = 7 } };
            _service.Setup(s => s.GetReviewsByUserAsync(7)).ReturnsAsync(list);

            var action = await _ctrl.GetReviewsByUser(7);
            var ok     = Assert.IsType<OkObjectResult>(action.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Review>>(ok.Value);

            Assert.Single(returned);
        }

        [Fact]
        public async Task GetReviewsByProduct_ReturnsOk_WithList()
        {
            var list = new[] { new Review { Id = 9, ProductId = 3 } };
            _service.Setup(s => s.GetReviewsByProductAsync(3)).ReturnsAsync(list);

            var action = await _ctrl.GetReviewsByProduct(3);
            var ok     = Assert.IsType<OkObjectResult>(action.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Review>>(ok.Value);

            Assert.Single(returned);
        }

        [Fact]
        public async Task ApproveReview_NotFound_ReturnsNotFound()
        {
            _service.Setup(s => s.ApproveReviewAsync(8)).ReturnsAsync(false);

            var result = await _ctrl.ApproveReview(8);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Review with ID 8 not found.", nf.Value);
        }

        [Fact]
        public async Task ApproveReview_Success_ReturnsOk()
        {
            _service.Setup(s => s.ApproveReviewAsync(3)).ReturnsAsync(true);

            var result = await _ctrl.ApproveReview(3);
            var ok     = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Review approved successfully.", ok.Value);
        }

        [Fact]
        public async Task RejectReview_NotFound_ReturnsNotFound()
        {
            _service.Setup(s => s.RejectReviewAsync(12)).ReturnsAsync(false);

            var result = await _ctrl.RejectReview(12);
            var nf     = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal("Review with ID 12 not found.", nf.Value);
        }

        [Fact]
        public async Task RejectReview_Success_ReturnsOk()
        {
            _service.Setup(s => s.RejectReviewAsync(4)).ReturnsAsync(true);

            var result = await _ctrl.RejectReview(4);
            var ok     = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Review rejected successfully.", ok.Value);
        }
    }