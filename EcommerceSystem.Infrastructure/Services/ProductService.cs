using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.DTOs;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Application.Helpers;
using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Domain.Entities;
using EcommerceSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceSystem.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(ProductDto dto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<PagedResponse<Product>> GetAsync(ProductQueryParams filters)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(filters.Name))
            query = query.Where(p => p.Name.ToLower().Contains(filters.Name.ToLower()));

        if (filters.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filters.MinPrice.Value);

        if (filters.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filters.MaxPrice.Value);

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToPagedResponseAsync(filters);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    //public async Task<PagedResult<Product>> GetPagedAsync(PaginationParams pagination)
    //{
    //    return await _context.Products
    //        .OrderBy(p => p.Name)
    //        .ToPagedResultAsync(pagination);
    //}
}
