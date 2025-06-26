using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Application.Dto
{
    public class OrderDto
    {
     
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
