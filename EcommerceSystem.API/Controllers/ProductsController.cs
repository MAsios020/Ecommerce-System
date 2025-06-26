using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.DTOs;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceSystem.API.Controllers;

[ApiController]
[Authorize]

[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
        
    [HttpGet]
    public async Task<ActionResult<PagedResponse<Product>>> GetALL([FromQuery] ProductQueryParams filters)
    {
        var result = await _productService.GetAsync(filters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> Create(ProductDto dto)
    {
        var created = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
