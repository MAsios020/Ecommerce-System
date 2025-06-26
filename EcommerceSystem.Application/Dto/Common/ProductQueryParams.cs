using EcommerceSystem.Application.Dto.Common;

namespace EcommerceSystem.Application.DTOs;

public class ProductQueryParams : PaginationFilter
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
