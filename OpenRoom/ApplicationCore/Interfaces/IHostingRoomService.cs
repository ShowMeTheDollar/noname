using ApplicationCore.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHostingRoomService
    {
     
        // 這是用 Entity 資料，更好的做法
        //Task<Room> GetHostingRooms(int roomId);
        List<Room> GetHostingRooms(int hostingId);
        Task <List<Room>> GetHostingRoomsAsync(int hostingId);
        //List<Room> GetRoomById(int roomId);

        //to fetch a single room given a hostingId and roomId
        Room GetRoomDetailsById(int roomId);
        public IEnumerable<RoomCategory> GetAllRoomCategories();
        void UpdateRoom(Room room);

        void DeleteRoom(int roomId);
    }


    
}
