using EcommerceSystem.Domain.Entities;
using EcommerceSystem.Domain.Enums;
using EcommerceSystem.Infrastructure.Data;

namespace EcommerceSystem.Infrastructure.Seed;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Id = Guid.Parse("b7eaa18e-b156-4d1b-b7aa-8bac6ace4dd8"),
                    FullName = "John Doe",
                    Email = "john@example.com",
                    IsAdmin = false
                },
                new User
                {
                    Id = Guid.Parse("548dd28f-79be-4804-b95a-295f4135f83b"),
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    IsAdmin = true
                }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            var prod1 = new Product
            {
                Id = Guid.Parse("0604cc0c-ccc6-4378-8ec6-1fc1480bf73a"),
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 25000,
                CreatedAt = DateTime.UtcNow
            };
            var prod2 = new Product
            {
                Id = Guid.Parse("955607c9-b989-4645-9774-4f0178478b74"),
                Name = "Mouse",
                Description = "Wireless Mouse",
                Price = 500,
                CreatedAt = DateTime.UtcNow
            };

            context.Products.AddRange(prod1, prod2);

            context.Inventories.AddRange(
                new Inventory { Id = Guid.NewGuid(), ProductId = prod1.Id, Quantity = 10 },
                new Inventory { Id = Guid.NewGuid(), ProductId = prod2.Id, Quantity = 50 }
            );
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var userId = Guid.Parse("b7eaa18e-b156-4d1b-b7aa-8bac6ace4dd8");
            var product1 = context.Products.First(p => p.Name == "Laptop");
            var product2 = context.Products.First(p => p.Name == "Mouse");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow.Date,
                Status = OrderStatus.Completed,
                OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = product1.Id,
                Quantity = 1,
                UnitPrice = product1.Price
            },
            new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = product2.Id,
                Quantity = 2,
                UnitPrice = product2.Price
            }
        }
            };

            context.Orders.Add(order);

            context.SaveChanges();
        }


       
    }
}
