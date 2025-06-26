using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.DTOs;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Domain.Entities;

namespace EcommerceSystem.Application.Interfaces;

public interface IProductService
{
    Task<PagedResponse<Product>> GetAsync(ProductQueryParams filters);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(ProductDto dto);
    

}