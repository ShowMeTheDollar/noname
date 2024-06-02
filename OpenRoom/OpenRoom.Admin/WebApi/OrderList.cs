using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using isRock.LineBot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace OpenRoom.Admin.WebApi
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderListController : ControllerBase
    {
        private readonly OpenRoomContext _context;

        public OrderListController(OpenRoomContext context)
        {
            _context = context;
        }
        // GET: api/RoomSourceList
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            // Fetch members from the database
            var orders = await _context.Orders.ToListAsync();

            // Transform the data to the DTO
            var orderDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                RoomID = o.RoomId,
                CustomerCount = o.CustomerCount,
                MemberID = o.MemberId,
                ReceiptNo = o.ReceiptNo,
                OrderNo = o.OrderNo,
                TotalPrice = o.TotalPrice
            }).ToList();

            // Return the DTOs
            return Ok(orderDto);
        }

        // 訂單圓餅圖
        [HttpGet("GetOrderRoomCategory")]
        public async Task<IActionResult> GetOrderRoomCategory()
        {
            try
            {
                var roomCategoryCount = await _context.Orders
                    .Include(o => o.Room)
                    .ThenInclude(r => r.RoomCategory)
                    .GroupBy(o => o.Room.RoomCategoryId)
                    .Select(g => new
                    {
                        RoomCategoryId = g.Key,
                        RoomCategoryName = g.First().Room.RoomCategory.RoomCategory1,
                        Count = g.Count()
                    })
                    .ToListAsync();

                return Ok(roomCategoryCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"發生錯誤 {ex.Message}");
            }
        }
    }
}
