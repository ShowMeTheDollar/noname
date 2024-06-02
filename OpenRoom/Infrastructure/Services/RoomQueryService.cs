using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RoomQueryService : IRoomQueryService
    {
        private readonly OpenRoomContext _context;
        public RoomQueryService(OpenRoomContext openRoomContext)
        {
            _context = openRoomContext;
        }

        public async Task<Room> GetRoomDetails(int roomId)
        {
            var room = await _context.Rooms
                        .Include(r => r.RoomImages)
                        .Include(r => r.RoomPrices)
                        .Include(r => r.Orders)
                        .Include(r => r.RoomCategory)
                        .Include(r => r.RoomAmenities)
                        .ThenInclude(ra => ra.Amentity)
                        .Include(r => r.Member)
                        .ThenInclude(m => m.LanguageSpeakers)
                        .Include(r => r.WishlistDetails)
                        .ThenInclude(w => w.Wishlist)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.Id == roomId);

            return room;
        }
    }
}
