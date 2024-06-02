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
    public class RoomPriceQueryService :IRoomPriceQueryService
    {
        private readonly OpenRoomContext _context;

        public RoomPriceQueryService(OpenRoomContext context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetRoomsByMemberId(int memberId)
        {
            return await _context.Rooms
                .Where(r => r.MemberId == memberId && !r.IsDelete)
                .ToListAsync();
        }

        public async Task<List<RoomPrice>> GetIndividualRoomPrice(int roomId)
        {
            return await _context.RoomPrices
                .Where(r => r.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<Room> GetRoomPrice(int roomId)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);
            return room;
        }
    }
}
