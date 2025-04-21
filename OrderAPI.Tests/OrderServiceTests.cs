using Microsoft.EntityFrameworkCore;
using Moq;
using OrderAPI.ApplicationCore.Contracts.Services;
using OrderAPI.ApplicationCore.Entities;
using OrderAPI.Infrastructure.Data;
using OrderAPI.Infrastructure.Services;

namespace OrderAPI.Tests;

 public class OrderServiceTests : IDisposable
    {
        private readonly Mock<IOrderRepository> _repoMock;
        private readonly OrderDbContext           _dbContext;
        private readonly OrderService             _svc;

        public OrderServiceTests()
        {
            // In-Memory EF setup (same as before)
            var opts = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new OrderDbContext(opts);
            _dbContext.Orders.Add(new Order { Id = 1, Order_Status = "Pending", Order_Date = DateTime.UtcNow });
            _dbContext.SaveChanges();

            // **Mock the IOrderRepository** interface, not IRepository<Order>**
            _repoMock = new Mock<IOrderRepository>();
            _svc      = new OrderService(_repoMock.Object, _dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        // ---- Tests for the EF‐based methods ----

        [Fact]
        public async Task MarkOrderCompletedAsync_ExistingOrder_SetsStatusAndPersists()
        {
            // Act
            var result = await _svc.MarkOrderCompletedAsync(1);

            // Assert
            Assert.True(result);
            var updated = await _dbContext.Orders.FindAsync(1);
            Assert.Equal("Completed" as string, updated!.Order_Status);
        }

        [Fact]
        public async Task CancelOrderAsync_ExistingOrder_SetsStatusAndPersists()
        {
            // Act
            var result = await _svc.CancelOrderAsync(2);

            // Assert
            Assert.True(result);
            var updated = await _dbContext.Orders.FindAsync(2);
            Assert.Equal("Cancelled", updated!.Order_Status);
        }

        [Fact]
        public async Task CancelOrderAsync_AlreadyCompleted_ReturnsFalse()
        {
            // Arrange
            // mark order 1 as Completed first
            var o1 = await _dbContext.Orders.FindAsync(1);
            o1!.Order_Status = "Completed";
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _svc.CancelOrderAsync(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetOrderStatusAsync_ExistingOrder_ReturnsCurrentStatus()
        {
            // Arrange
            // order 2 is still "Pending"
            // Act
            var status = await _svc.GetOrderStatusAsync(2);

            // Assert
            Assert.Equal("Pending", status);
        }


        // ---- Tests for the repository‐backed methods ----

        [Fact]
        public async Task GetOrderByIdAsync_UsesRepository()
        {
            // Arrange
            var dummy = new Order { Id = 99 };
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync(dummy);

            // Act
            var result = await _svc.GetOrderByIdAsync(99);

            // Assert
            Assert.Same(dummy, result);
            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }
        

        [Fact]
        public async Task InsertAsync_UsesRepository()
        {
            var o = new Order();
            _repoMock.Setup(r => r.InsertAsync(o)).ReturnsAsync(42);

            var id = await _svc.InsertAsync(o);

            Assert.Equal(42, id);
            _repoMock.Verify(r => r.InsertAsync(o), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UsesRepository()
        {
            var o = new Order { Id = 7 };
            _repoMock.Setup(r => r.UpdateAsync(o)).ReturnsAsync(1);

            var rows = await _svc.UpdateAsync(o);

            Assert.Equal(1, rows);
            _repoMock.Verify(r => r.UpdateAsync(o), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UsesRepository()
        {
            _repoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(1);

            var rows = await _svc.DeleteAsync(7);

            Assert.Equal(1, rows);
            _repoMock.Verify(r => r.DeleteAsync(7), Times.Once);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsProperPage_FromDbContext()
        {
            // Act: page 1, size 1 → should return the most recent order
            var page1 = (await _svc.GetAllOrdersAsync(page:1, size:1)).ToList();

            // Assert
            Assert.Single(page1);
            // Highest Order_Date was for Id = 2 in our seed
            Assert.Equal(2, page1[0].Id);
        }
    }