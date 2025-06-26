using EcommerceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5);
    }

}
