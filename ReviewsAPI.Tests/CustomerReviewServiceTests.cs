using Moq;
using ReviewsAPI.ApplicationCore.Contracts.Repositories;
using ReviewsAPI.ApplicationCore.Entities;
using ReviewsAPI.Infrastructure.Services;

namespace ReviewsAPI.Tests;

public class CustomerReviewServiceTests
    {
        private readonly Mock<ICustomerReviewRepository> _repoMock;
        private readonly CustomerReviewService          _svc;

        public CustomerReviewServiceTests()
        {
            _repoMock = new Mock<ICustomerReviewRepository>();
            _svc      = new CustomerReviewService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllReviews()
        {
            // Arrange
            var reviews = new[]
            {
                new Review { Id = 1, Status = "Pending" },
                new Review { Id = 2, Status = "Pending" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reviews);

            // Act
            var result = (await _svc.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, rv => rv.Id == 1);
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsReview()
        {
            // Arrange
            var review = new Review { Id = 42, Status = "Pending" };
            _repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(review);

            // Act
            var result = await _svc.GetByIdAsync(42);

            // Assert
            Assert.Same(review, result);
            _repoMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_UnknownId_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Review?)null);

            var result = await _svc.GetByIdAsync(99);

            Assert.Null(result);
            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_CallsRepository_ReturnsNewId()
        {
            // Arrange
            var newReview = new Review { ProductId = 1, CustomerId = 2, RatingValue = 5, Comment = "Great!" };
            _repoMock.Setup(r => r.InsertAsync(newReview)).ReturnsAsync(123);

            // Act
            var id = await _svc.CreateAsync(newReview);

            // Assert
            Assert.Equal(123, id);
            _repoMock.Verify(r => r.InsertAsync(newReview), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository_ReturnsRowsAffected()
        {
            var existing = new Review { Id = 7, Status = "Pending" };
            _repoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(1);

            var rows = await _svc.UpdateAsync(existing);

            Assert.Equal(1, rows);
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository_ReturnsRowsAffected()
        {
            _repoMock.Setup(r => r.DeleteAsync(5)).ReturnsAsync(1);

            var rows = await _svc.DeleteAsync(5);

            Assert.Equal(1, rows);
            _repoMock.Verify(r => r.DeleteAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByUserAsync_ReturnsCorrectSubset()
        {
            var list = new[]
            {
                new Review { Id = 1, CustomerId = 10 },
                new Review { Id = 2, CustomerId = 10 }
            };
            _repoMock.Setup(r => r.GetReviewsByUserAsync(10)).ReturnsAsync(list);

            var result = (await _svc.GetReviewsByUserAsync(10)).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(10, r.CustomerId));
            _repoMock.Verify(r => r.GetReviewsByUserAsync(10), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByProductAsync_ReturnsCorrectSubset()
        {
            var list = new[]
            {
                new Review { Id = 3, ProductId = 20 },
                new Review { Id = 4, ProductId = 20 }
            };
            _repoMock.Setup(r => r.GetReviewsByProductAsync(20)).ReturnsAsync(list);

            var result = (await _svc.GetReviewsByProductAsync(20)).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(20, r.ProductId));
            _repoMock.Verify(r => r.GetReviewsByProductAsync(20), Times.Once);
        }

        [Fact]
        public async Task ApproveReviewAsync_ExistingReview_SetsStatusAndReturnsTrue()
        {
            // Arrange
            var review = new Review { Id = 8, Status = "Pending" };
            _repoMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(review);
            _repoMock.Setup(r => r.UpdateAsync(review)).ReturnsAsync(1);

            // Act
            var ok = await _svc.ApproveReviewAsync(8);

            // Assert
            Assert.True(ok);
            Assert.Equal("Approved", review.Status);
            _repoMock.Verify(r => r.GetByIdAsync(8), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(review), Times.Once);
        }

        [Fact]
        public async Task ApproveReviewAsync_NonExistingReview_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Review?)null);

            var ok = await _svc.ApproveReviewAsync(99);

            Assert.False(ok);
            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task RejectReviewAsync_ExistingReview_SetsStatusAndReturnsTrue()
        {
            var review = new Review { Id = 9, Status = "Pending" };
            _repoMock.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(review);
            _repoMock.Setup(r => r.UpdateAsync(review)).ReturnsAsync(1);

            var ok = await _svc.RejectReviewAsync(9);

            Assert.True(ok);
            Assert.Equal("Rejected", review.Status);
            _repoMock.Verify(r => r.GetByIdAsync(9), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(review), Times.Once);
        }

        [Fact]
        public async Task RejectReviewAsync_NonExistingReview_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Review?)null);

            var ok = await _svc.RejectReviewAsync(77);

            Assert.False(ok);
            _repoMock.Verify(r => r.GetByIdAsync(77), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }
    }