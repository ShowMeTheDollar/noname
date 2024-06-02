using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Services
{// 從資料庫中查詢訂單資料
    public class HostingQueryService : IHostingQueryService
    {


        private readonly OpenRoomContext _context;

        public HostingQueryService(OpenRoomContext openRoomContext)
        {
            _context = openRoomContext;
        }

        public List<Order> GetHostingRoomOrdersByMember(int memberId)//房東
        {
            //return _context.Members
            //               .Where(m => m.Id == memberId)
            //               .Include(o => o.Rooms)
            //               .ThenInclude(r => r.Orders)
            //               .SelectMany(o => o.Orders)
            //               .AsNoTracking()
            //               .ToList();
            //return _context.Orders
            //       .Include(o => o.Room)
            //       .ThenInclude(r => r.Member)
            //       .Where(o => o.Room.MemberId == memberId &&
            //                  (o.OrderStatus == (int)OrderStatus.Active || o.OrderStatus == (int)OrderStatus.Ongoing))// 房東的 MemberId 與 Room 關聯
            //       .AsNoTracking()
            //       .ToList();

            // Fetch the orders eagerly loading Room and Member associated with each Room
            var orders = _context.Orders
                           .Include(o => o.Room)
                               .ThenInclude(r => r.Member)
                           .Where(o => o.Room.MemberId == memberId &&
                                      (o.OrderStatus == (int)OrderStatus.Active || o.OrderStatus == (int)OrderStatus.Ongoing))
                           .AsNoTracking()
                           .ToList();

            // Populate the guest name by looking up each order's Member information
            foreach (var order in orders)
            {
                // Ensure we have a valid Member reference in the order before trying to access it
                if (order.Member == null)
                {
                    // This order doesn't have a related Member loaded; attempt to load it now.
                    order.Member = _context.Members.Find(order.MemberId);
                }

                // If we have a Member loaded, either by ThenInclude or by explicitly loading it above, we can continue.
                if (order.Member != null)
                {
                    var guest = _context.Members.FirstOrDefault(m => m.Id == order.MemberId);
                    if (guest != null)
                    {
                        order.Member.FirstName = guest.FirstName;
                        order.Member.LastName = guest.LastName;
                    }
                    else
                    {
                        // Log or handle the case where a Member with the given Id doesn't exist
                        // Set a default guest name to indicate that the member was not found
                        order.Member = new Member { FirstName = "Unknown", LastName = "Guest" };
                    }
                }
            }

            return orders;

        }



        public List<Room> GetHostingRooms(int hostingId)
        {
            return _context.Rooms
                .Include(r => r.RoomImages)
                .Include(r => r.RoomCategory)
                .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amentity)
                .Where(r => r.MemberId == hostingId)
                .AsNoTracking()// 如果我今天只是需要從資料庫查詢東西顯示到我的會員訂單畫面上就可以使用.AsNoTracking()
                .ToList();
        }

        //public Room GetRoomByIdAndHostingId(int hostingId, int roomId)
        //{
        //    return _context.Rooms
        //        .Include(r => r.RoomImages)
        //        .Include(r => r.RoomCategory)
        //        .Include(r => r.RoomAmenities).ThenInclude(ra => ra.Amentity)
        //        .Where(r => r.MemberId == hostingId && r.Id == roomId)
        //        .AsNoTracking()
        //        .FirstOrDefault();
        //}

        public Room GetRoomDetailsById(int roomId)
        {
            // 首先，从数据库加载房间及其相关数据
            return _context.Rooms
                .Include(r => r.RoomImages)
                .Include(r => r.RoomCategory)
                .Include(r => r.RoomAmenities).ThenInclude(ra => ra.Amentity)
                .Where(r => r.Id == roomId)
                .AsNoTracking()
                .FirstOrDefault();
          
        }

        public void UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }

    }



}

