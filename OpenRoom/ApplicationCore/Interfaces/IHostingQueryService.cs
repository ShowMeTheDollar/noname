using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHostingQueryService
    {
        public List<Room> GetHostingRooms(int hostingId);
        public List<Order> GetHostingRoomOrdersByMember(int memberId);

        //single Room instead of a list, since you're fetching details for one room
        //Room GetRoomByIdAndHostingId(int hostingId, int roomId);
        public Room GetRoomDetailsById(int roomId);
        void UpdateRoom(Room room);

    }
}
