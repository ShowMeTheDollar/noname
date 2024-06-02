using ApplicationCore.DTO;
using ApplicationCore.Entities;
using Infrastructure.Data;
using isRock.LineBot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenRoom.Admin.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishList : ControllerBase
    {
        private readonly OpenRoomContext _context;

        public WishList(OpenRoomContext context)
        {
            _context = context;
        }
        // 心願單房源表格
        [HttpGet("GetWishs")]
        public async Task<IActionResult> GetWishs()
        {
            var wishs = await _context.WishlistDetails
                .Where(w => w.Wishlist != null && w.Wishlist.MemberId != null) // Wishlist 或 MemberID 為 null 的就不要顯示
                .Select(w => new WishlistDetailDto
                {
                    WishDetailID = w.Id,
                    MemberID = w.Wishlist.MemberId,
                    WishlistID = w.WishlistId,
                    RoomID = w.RoomId,
                    RoomName = w.Room.RoomName,
                    RoomCategory = w.Room.RoomCategory.RoomCategory1
                })
                .ToListAsync();

            return Ok(wishs);
        }

        // 心願單圓餅圖
        [HttpGet("GetWishRoomCategory")]
        public async Task<IActionResult> GetWishRoomCategory()
        {// 抓 WishlistDetails 中收藏的所有 Room ,用來統計房間種類的數量( 最熱門的種類 )
            var wishRooms = await _context.WishlistDetails
                .Include(w => w.Room)
                .ThenInclude(r => r.RoomCategory)
                .Select(w => w.Room.RoomCategory.RoomCategory1)
                .Distinct()
                .Select(categoryName => new
                {
                    Category = categoryName,
                    Count = _context.WishlistDetails.Count(w => w.Room.RoomCategory.RoomCategory1 == categoryName)
                })
                .OrderByDescending(x => x.Count) // 從最熱門的排序
                .ToListAsync();

            return Ok(wishRooms);
        }

    }
}
