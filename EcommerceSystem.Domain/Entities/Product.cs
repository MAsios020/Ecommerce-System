using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }  
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One product to  one inventory ( 1 to 1 ) 
        public Inventory? Inventory { get; set; }

        // One product to many OrderItems ( 1 to many )
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
