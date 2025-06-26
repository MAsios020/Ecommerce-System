using EcommerceSystem.Application.Dto.Common;
using EcommerceSystem.Application.DTOs.Common;

namespace EcommerceSystem.Application.Helpers;

public static class IQueryableExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query,
        PaginationFilter filter)
    {
        var count = query.Count();
        var items = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return new PagedResponse<T>
        {
            CurrentPage = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling(count / (double)filter.PageSize),
            Items = items
        };
    }
}
