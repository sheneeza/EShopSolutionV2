using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderAPI.ApplicationCore.Contracts.Services;
using OrderAPI.ApplicationCore.Entities;
using OrderAPI.ApplicationCore.Models;
using OrderAPI.Controllers;

namespace OrderAPI.Tests;

public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _serviceMock;
        private readonly Mock<IMapper>       _mapperMock;
        private readonly OrderController     _ctrl;

        public OrderControllerTests()
        {
            _serviceMock = new Mock<IOrderService>();
            _mapperMock  = new Mock<IMapper>();
            _ctrl        = new OrderController(_serviceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOk_WithListOfOrders()
        {
            // Arrange
            var fakeOrders = new[]
            {
                new Order { Id = 1 },
                new Order { Id = 2 }
            };
            _serviceMock
                .Setup(s => s.GetAllOrdersAsync(2, 5))
                .ReturnsAsync(fakeOrders);

            // Act
            var result = await _ctrl.GetAllOrders(page: 2, size: 5);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<Order>>(ok.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public async Task SaveOrder_OnSuccess_ReturnsOkMessage()
        {
            // Arrange
            var dto = new OrderModel { /* fill required props */ };
            var mapped = new Order();
            _mapperMock
                .Setup(m => m.Map<Order>(dto))
                .Returns(mapped);
            _serviceMock
                .Setup(s => s.InsertAsync(mapped))
                .ReturnsAsync(123);

            // Act
            var result = await _ctrl.SaveOrder(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order placed.", ok.Value);
        }

        [Fact]
        public async Task SaveOrder_OnFailure_Returns500()
        {
            // Arrange
            _mapperMock.Setup(m => m.Map<Order>(It.IsAny<OrderModel>()))
                       .Returns(new Order());
            _serviceMock.Setup(s => s.InsertAsync(It.IsAny<Order>()))
                        .ReturnsAsync(0);

            // Act
            var result = await _ctrl.SaveOrder(new OrderModel());

            // Assert
            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
            Assert.Equal("Failed to place order.", obj.Value);
        }

        [Fact]
        public async Task CheckOrderHistory_ReturnsOk_WithOrder()
        {
            // Arrange
            var order = new Order { Id = 99 };
            _serviceMock.Setup(s => s.GetOrderByIdAsync(99))
                        .ReturnsAsync(order);

            // Act
            var result = await _ctrl.CheckOrderHistory(customerId: 99);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(order, ok.Value);
        }

        [Fact]
        public async Task CheckOrderStatus_WhenFound_ReturnsOk()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetOrderStatusAsync(5))
                        .ReturnsAsync("Pending");

            // Act
            var result = await _ctrl.CheckOrderStatus(orderId: 5);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Pending", ok.Value);
        }

        [Fact]
        public async Task CheckOrderStatus_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetOrderStatusAsync(It.IsAny<int>()))
                        .ReturnsAsync((string?)null);

            // Act
            var result = await _ctrl.CheckOrderStatus(7);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CancelOrder_Success_ReturnsOk()
        {
            // Arrange
            _serviceMock.Setup(s => s.CancelOrderAsync(3))
                        .ReturnsAsync(true);

            // Act
            var result = await _ctrl.CancelOrder(orderId: 3);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order cancelled.", ok.Value);
        }

        [Fact]
        public async Task CancelOrder_Failure_ReturnsBadRequest()
        {
            // Arrange
            _serviceMock.Setup(s => s.CancelOrderAsync(It.IsAny<int>()))
                        .ReturnsAsync(false);

            // Act
            var result = await _ctrl.CancelOrder(orderId: 8);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Unable to cancel order.", bad.Value);
        }

        [Fact]
        public async Task CompleteOrder_Success_ReturnsOk()
        {
            // Arrange
            _serviceMock.Setup(s => s.MarkOrderCompletedAsync(4))
                        .ReturnsAsync(true);

            // Act
            var result = await _ctrl.CompleteOrder(orderId: 4);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order marked as completed.", ok.Value);
        }

        [Fact]
        public async Task CompleteOrder_Failure_ReturnsBadRequest()
        {
            // Arrange
            _serviceMock.Setup(s => s.MarkOrderCompletedAsync(It.IsAny<int>()))
                        .ReturnsAsync(false);

            // Act
            var result = await _ctrl.CompleteOrder(orderId: 10);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Update failed.", bad.Value);
        }

        [Fact]
        public async Task UpdateOrder_NullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _ctrl.UpdateOrder(1, updatedOrderDto: null!);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid order data.", bad.Value);
        }

        [Fact]
        public async Task UpdateOrder_NotFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetOrderByIdAsync(5))
                        .ReturnsAsync((Order?)null);

            // Act
            var result = await _ctrl.UpdateOrder(5, new OrderModel());

            // Assert
            var nf = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Order not found.", nf.Value);
        }

        [Fact]
        public async Task UpdateOrder_Success_ReturnsOk()
        {
            // Arrange
            var existing = new Order { Id = 6 };
            var dto      = new OrderModel { /* props */ };

            _serviceMock.Setup(s => s.GetOrderByIdAsync(6))
                        .ReturnsAsync(existing);
            _mapperMock
                .Setup(m => m.Map(dto, existing))
                .Callback<OrderModel, Order>((d, e) => { /* simulate mapping */ });
            _serviceMock.Setup(s => s.UpdateAsync(existing))
                        .ReturnsAsync(1);

            // Act
            var result = await _ctrl.UpdateOrder(6, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order updated.", ok.Value);
        }

        [Fact]
        public async Task UpdateOrder_UpdateFails_Returns500()
        {
            // Arrange
            var existing = new Order { Id = 7 };
            _serviceMock.Setup(s => s.GetOrderByIdAsync(7)).ReturnsAsync(existing);
            _mapperMock.Setup(m => m.Map(It.IsAny<OrderModel>(), existing));
            _serviceMock.Setup(s => s.UpdateAsync(existing)).ReturnsAsync(0);

            // Act
            var result = await _ctrl.UpdateOrder(7, new OrderModel());

            // Assert
            var obj = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, obj.StatusCode);
            Assert.Equal("Failed to update order.", obj.Value);
        }
    }