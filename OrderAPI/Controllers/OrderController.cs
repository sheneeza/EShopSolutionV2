using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.ApplicationCore.Contracts.Services;
using OrderAPI.ApplicationCore.Entities;
using OrderAPI.ApplicationCore.Models;
using OrderAPI.Utility;

namespace OrderAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly OrderCompleted _orderCompleted;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
        _orderCompleted = new OrderCompleted();
    }
        
    [HttpGet("GetAllOrders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders(int page = 1, int size = 10)
    {
        var orders = await _orderService.GetAllOrdersAsync(page, size);
        return Ok(orders);
    }
    
    [HttpPost("SaveOrder")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> SaveOrder([FromBody] OrderModel orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        var result = await _orderService.InsertAsync(order);
        return result > 0 ? Ok("Order placed.") : StatusCode(500, "Failed to place order.");
    }


    [HttpGet("CheckOrderHistory")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CheckOrderHistory(int customerId)
    {
        var history = await _orderService.GetOrderByIdAsync(customerId);
        return Ok(history);
    }
    
    [HttpGet("CheckOrderStatus")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CheckOrderStatus(int orderId)
    {
        var status = await _orderService.GetOrderStatusAsync(orderId);
        return status == null ? NotFound() : Ok(status);
    }

    [HttpPut("CancelOrder")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var result = await _orderService.CancelOrderAsync(orderId);
        return result ? Ok("Order cancelled.") : BadRequest("Unable to cancel order.");
    }

    [HttpPost("OrderCompleted")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        // 1) Mark as completed
        var success = await _orderService.MarkOrderCompletedAsync(orderId);
        if (!success)
            return BadRequest("Update failed.");

        // 2) Reload full order (with details)
        var order = await _orderService.GetOrderByIdAsync(orderId);
        if (order == null)
            return NotFound($"Order {orderId} not found after completion.");

        // 3) Event payload
        var evt = new
        {
            order.Id,
            OrderDate        = order.Order_Date,
            order.CustomerId,
            order.CustomerName,
            Payment = new
            {
                order.PaymentMethodId,
                order.PaymentName
            },
            Shipping = new
            {
                order.ShippingAddress,
                order.ShippingMethod
            },
            order.BillingAmount,
            OrderStatus   = order.Order_Status,
            Items = order.Order_Details.Select(d => new
            {
                d.Id,
                d.Product_Id,
                d.Product_name,
                d.Qty,
                d.Price,
                d.Discount
            })
        };

        // 4) Serialize & publish
        var message = JsonSerializer.Serialize(evt);
        await _orderCompleted.AddMessageToQueueAsync(message);

        return Ok("Order marked as completed and event published.");
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderModel updatedOrderDto)
    {
        if (updatedOrderDto == null)
            return BadRequest("Invalid order data.");

        var existingOrder = await _orderService.GetOrderByIdAsync(id);
        if (existingOrder == null)
            return NotFound("Order not found.");

        _mapper.Map(updatedOrderDto, existingOrder); 

        var result = await _orderService.UpdateAsync(existingOrder);
        return result > 0 ? Ok("Order updated.") : StatusCode(500, "Failed to update order.");
    }
    
    [HttpPut("{orderId}/shipping-status")]
    public async Task<IActionResult> UpdateShippingStatus(
        int orderId,
        [FromBody] OrderShippingUpdateModel dto)
    {
        if (dto == null || dto.OrderId != orderId)
            return BadRequest("Order ID mismatch.");
        
        var existingOrder = await _orderService.GetOrderByIdAsync(orderId);
        if (existingOrder == null)
            return NotFound($"Order {orderId} not found.");
        
        _mapper.Map(dto, existingOrder);
        
        var updated = await _orderService.UpdateAsync(existingOrder);
        if (updated > 0)
            return Ok("Shipping status updated.");
            
        return StatusCode(500, "Failed to update shipping status.");
    }

}