using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.Dto.Common;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Domain.Entities;
using EcommerceSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDto dto, Guid userId);
        Task<OrderViewDto?> GetOrderByIdAsync(Guid id, Guid userId, bool isAdmin);



        Task<PagedResponse<OrderViewDto>> GetAsync(OrderQueryParams filters);

        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
        Task<DailySalesReportDto> GetDailySalesReportAsync();



    }
}
