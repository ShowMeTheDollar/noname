using ApplicationCore.Interfaces;
using OpenRoom.Web.Services.OrderService.DTOs;
using System.Globalization;

namespace OpenRoom.Web.Services.OrderService
{
    public class OrderService
    {
        private readonly IOrderQueryService _orderQueryService;
        private readonly IRoomQueryService _roomQueryService;

        public OrderService(IOrderQueryService orderQueryService, IRoomQueryService roomQueryService)
        {
            _orderQueryService = orderQueryService;
            _roomQueryService = roomQueryService;
        }

        public OrderingViewModel GetOrderInfo(OrderDTO orderInfo) 
        {
            var pricePerNightDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(orderInfo.PricePerNight);

            return new OrderingViewModel()
            {
                RoomId = orderInfo.RoomId,
                RoomName = orderInfo.RoomName,
                RoomImg = orderInfo.RoomImg,
                RoomAddress = orderInfo.RoomAddress,    
                RoomCategory = orderInfo.RoomCategory,

                CheckIn = DateTime.ParseExact(orderInfo.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                CheckOut = DateTime.ParseExact(orderInfo.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                GuestCount = orderInfo.GuestCount,
           
                PricePerNight = pricePerNightDict,
                PriceTotal = orderInfo.PriceTotal,

                Rating = 4.87m, // 未來做評論功能後, 改為動態資料
                ReviewCount = 124, // 未來做評論功能後, 改為動態資料

                GuestCountLimitation = orderInfo.GuestCountLimitaion,
            };
        }


        public async Task<OrderConfirmationViewModel> GetOrderConfirmation(string orderNo)
        {
            var order = await _orderQueryService.GetOrderDetails(orderNo);
            if (order == null)
            {
                return null;
            }

            var room = await _roomQueryService.GetRoomDetails(order.RoomId);
            if (room == null)
            {
                return null;
            }

            return new OrderConfirmationViewModel()
            {
                OrderNo = order.OrderNo,
                RoomId = order.RoomId,
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,
                OrderGuestCount = order.CustomerCount,
                TotalPrice = Math.Round(order.TotalPrice),
                GuestEmail = order.Member.Email,

                RoomName = room.RoomName,
                RoomImg = room.RoomImages.Select(image => image.ImageUrl).First(),
                RoomAddress = $"{room.CityName}{room.DistrictName}{room.Street}",
                HostImg = room.Member.Avatar ?? "/images/user/default_avatar.png",
                HostName = room.Member.FirstName,
                //若 CheckInStartTime 為 null, 則預設為下午 14:00
                CheckInTime = room.CheckInStartTime != null
                              ? TimeOnly.FromDateTime((DateTime)room.CheckInStartTime)
                              : TimeOnly.FromDateTime(new DateTime(2024, 4, 1, 14, 0, 0)),
                //若 CheckInEndTime 為 null, 則預設為早上 12:00
                CheckOutTime = room.CheckInEndTime != null
                              ? TimeOnly.FromDateTime((DateTime)room.CheckInEndTime)
                              : TimeOnly.FromDateTime(new DateTime(2024, 4, 1, 12, 0, 0)),
                GuestCount = room.GuestCount
            };
        }

        public OrderCancellationViewModel CancelOrder(int OrderID)
        {
            return new OrderCancellationViewModel()
            {
                // 暫不實作
            };
        }

        public OrderModificationViewModel ModifyOrder(int OrderID)
        {
            return new OrderModificationViewModel()
            {
                // 暫不實作
            };
        }
    }
}
