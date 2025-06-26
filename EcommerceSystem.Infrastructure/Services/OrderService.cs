using EcommerceSystem.Application.Dto;
using EcommerceSystem.Application.Dto.Common;
using EcommerceSystem.Application.DTOs.Common;
using EcommerceSystem.Application.Helpers;
using EcommerceSystem.Application.Interfaces;
using EcommerceSystem.Domain.Entities;
using EcommerceSystem.Domain.Enums;
using EcommerceSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Infrastructure.Services
{

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(OrderDto dto, Guid userId)
        {
            //Check That User Exist
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new ArgumentException("User not found.");

            // Check Product avalibltiy 
            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Include(p => p.Inventory)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != dto.Items.Count)
                throw new ArgumentException("One or more products are invalid.");

            foreach (var item in dto.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                if (product.Inventory == null || product.Inventory.Quantity < item.Quantity)
                    throw new InvalidOperationException($"Insufficient inventory for product: {product.Name}");
            }

            // Create The Order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);

                // Decreasing the stock after Create order
                if (product.Inventory != null)
                    product.Inventory.Quantity -= item.Quantity;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<OrderViewDto?> GetOrderByIdAsync(Guid id, Guid userId ,bool isAdmin)
        {
            var query = _context.Orders
       .Include(o => o.OrderItems)
       .ThenInclude(oi => oi.Product)
       .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(o => o.UserId == userId);
            }

            var order = await query.FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? null : OrderMapper.ToViewDto(order);
        }

        public async Task<PagedResponse<OrderViewDto>> GetAsync(OrderQueryParams filters)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (filters.UserId.HasValue)
                query = query.Where(o => o.UserId == filters.UserId);

            if (filters.Status.HasValue)
                query = query.Where(o => o.Status == filters.Status.Value);

            if (filters.DateFrom.HasValue)
                query = query.Where(o => o.CreatedAt >= filters.DateFrom.Value);

            if (filters.DateTo.HasValue)
                query = query.Where(o => o.CreatedAt <= filters.DateTo.Value);

            query = query.OrderByDescending(o => o.CreatedAt);

            var result = await query.ToPagedResponseAsync(filters);

            return new PagedResponse<OrderViewDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(OrderMapper.ToViewDto)
            };
        }



        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DailySalesReportDto> GetDailySalesReportAsync()
        {
            var today = DateTime.UtcNow.Date;
            var orders = await _context.Orders
                .Where(o => o.CreatedAt.Date == today)
                .Include(o => o.OrderItems)
                .ToListAsync();

            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.OrderItems.Sum(i => i.Quantity * i.UnitPrice));

            return new DailySalesReportDto
            {
                Date = today,
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue
            };
        }

       

        public static class OrderMapper
        {
            public static OrderViewDto ToViewDto(Order order)
            {
                return new OrderViewDto
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    Status = order.Status.ToString(),
                    Items = order.OrderItems.Select(oi => new OrderItemViewDto
                    {
                        ProductName = oi.Product?.Name ?? "Unknown",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                };
            }
        }
    }
}
