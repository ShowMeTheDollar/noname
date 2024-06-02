using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Enums;


namespace Infrastructure.Services
{
    public class WishCardQueryService : IWishCardQueryService
    {
        private readonly OpenRoomContext _context;

        public WishCardQueryService(OpenRoomContext context)
        {
            _context = context;
        }
        // Wish
        public List<Wishlist> GetCardList(int memberId)
        {
            return _context.Wishlists
                .Include(w => w.Member)
                .Include(w => w.WishlistDetails)
                .ThenInclude(wd => wd.Room) 
                .ThenInclude(r => r.RoomImages) 
                .Where(w => w.MemberId == memberId)
                .ToList();
        }

        // WishMap
        public List<WishlistDetail> GetRooms(int wishlistId)
        {
            return _context.WishlistDetails
                .Where(wd => wd.WishlistId == wishlistId)
                .Include(wd => wd.Wishlist)
                .Include(wd => wd.Room.RoomCategory)
                .Include(wd => wd.Room.RoomImages)
                .Include(wd => wd.Room.Orders)
                .ThenInclude(o => o.RoomReview)
                .ToList();
        }
        public List<Order> GetOrderRoomInfo(int memberId)
        {
            return _context.Orders
                .Include(o => o.Room)
                .ThenInclude(r => r.RoomImages) 
                .Include(o => o.Room.RoomCategory) 
                .Include(o => o.Room.Member) 
                .Where(o => o.MemberId == memberId)
                .AsNoTracking()
                .ToList();
        }

        // Travel
        public List<Order> GetTravelCard(int memberId)
        {
            return _context.Orders
                .Include(o => o.Room)
                .ThenInclude(r => r.RoomImages)
                .Include(o => o.Room.RoomCategory)
                .Include(o => o.Room.Member)
                .Where(o => o.MemberId == memberId)
                .AsNoTracking()
                .ToList();
        }

        public async Task<Order> GetReceipt(int orderId)
        {
            var receipt = await _context.Orders
                .Include(o => o.Room)
                .ThenInclude(r => r.RoomImages)
                .Include(o => o.Room.RoomCategory)
                .Include(o => o.Room.Member)
                .Include(o => o.Member)
                .Include(o => o.Ecpays)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
                return receipt;
        }


    }
}
