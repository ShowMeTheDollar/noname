using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class HostingRoomOrderService : IHostingRoomOrderService
    {

        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<Room> _roomRepo;
        private readonly IHostingQueryService _queryService;
        public HostingRoomOrderService(IRepository<Member> memberRepo, IRepository<Order> orderRepo, IHostingQueryService queryService, IRepository<Room> roomRepo)
        {
            _memberRepo = memberRepo;
            _orderRepo = orderRepo;
            _queryService = queryService;
            _roomRepo = roomRepo;
        }


        public List<Order> GetHostingRoomOrdersByMember(int memberId)//two methods repo or ef
        {
            //repo
            // Step 1: Retrieve a list of room IDs for rooms owned by the landlord (memberId).
            var roomIds = _roomRepo.List(r => r.MemberId == memberId).Select(r => r.Id).ToList();

            // Step 2: Retrieve all orders for those rooms with Active or Ongoing status.
            var orders = _orderRepo.List(o => roomIds.Contains(o.RoomId) &&
                                              (o.OrderStatus == (int)OrderStatus.Active ||
                                               o.OrderStatus == (int)OrderStatus.Ongoing))
                                   .ToList();




            // Populate guest names from the Members repository,additional data needed for the view model
            foreach (var order in orders)
            {
                // might need to fetch the related data eagerly if it's not already included
                order.Room = _roomRepo.FirstOrDefault(r => r.Id == order.RoomId);
                var guest = _memberRepo.FirstOrDefault(m => m.Id == order.MemberId);
                if (guest != null)
                {
                    order.Member.FirstName = guest.FirstName;
                    order.Member.LastName = guest.LastName;
                }
            }

            //var orders = _queryService.GetHostingRoomOrdersByMember(memberId);//EF

            return orders;
        }



        //public List<Order> GetAllOrders()
        //{
        //    var roomOrder = _queryService.GetAllOrders();
        //    return roomOrder;
        //}

    }


}

