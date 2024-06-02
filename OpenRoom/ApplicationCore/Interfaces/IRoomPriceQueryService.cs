using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomPriceQueryService
    {
        Task<List<Room>> GetRoomsByMemberId(int memberId);

        Task<List<RoomPrice>> GetIndividualRoomPrice(int roomId);

        Task<Room> GetRoomPrice(int roomId);
    }
}
