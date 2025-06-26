using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.Dto.Common;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Application.Helpers;
using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // POST /api/orders
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<OrderViewDto>> Create([FromBody] OrderDto dto)
    {
        var userId = User.GetUserId();
        var order = await _orderService.CreateOrderAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    // GET /api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderViewDto>> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var isAdmin = User.IsInRole("Admin");

        var order = await _orderService.GetOrderByIdAsync(id, userId, isAdmin);
        if (order == null) return NotFound();

        return Ok(order);
    }



    [HttpGet("GetAll")]
    public async Task<ActionResult<PagedResponse<OrderViewDto>>> GetAllbyFilter([FromQuery] OrderQueryParams filters)
    {
        filters.UserId = User.GetUserId();

        var result = await _orderService.GetAsync(filters);
        return Ok(result);

        //sds
    }
}
