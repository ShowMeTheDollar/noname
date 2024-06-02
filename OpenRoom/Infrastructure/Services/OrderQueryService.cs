using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly OpenRoomContext _context;

        public OrderQueryService(OpenRoomContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderDetails(string orderNo)
        {
            var order = await _context.Orders
                         .Include(o => o.Room)
                         .Include(o => o.Member)
                         .Include(o => o.Ecpays)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(o => o.OrderNo == orderNo);
            return order;
        }
    }
}
