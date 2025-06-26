using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.Dto.Common;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Domain.Entities;
using EcommerceSystem.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
    
public class AdminController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;

    public AdminController(IOrderService orderService, IInventoryService inventoryService)
    {
        _orderService = orderService;
        _inventoryService = inventoryService;
    }

    // GET: /api/v1/admin/orders
    [HttpGet("orders/filter")]
    public async Task<ActionResult<PagedResponse<OrderViewDto>>> Filter([FromQuery] OrderQueryParams filters)
    {
        var result = await _orderService.GetAsync(filters);
        return Ok(result);
    }

    // PUT: /api/v1/admin/orders/{id}/status
    [HttpPut("orders/{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] OrderStatus newStatus)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, newStatus);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // GET: /api/v1/admin/inventory/low-stock
    [HttpGet("inventory/low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var lowStock = await _inventoryService.GetLowStockProductsAsync();
        return Ok(lowStock);
    }

    // GET: /api/v1/admin/reports/daily
    [HttpGet("reports/daily")]
    public async Task<IActionResult> GetDailyReport()
    {
        var report = await _orderService.GetDailySalesReportAsync();
        return Ok(report);
    }
}
