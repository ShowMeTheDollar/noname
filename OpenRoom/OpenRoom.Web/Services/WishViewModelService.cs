using OpenRoom.Web.ViewModels;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using OpenRoom.Web.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Enums;
using OpenRoom.Web.ViewModels;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OpenRoom.Web.Services
{
    public class WishViewModelService : IWishViewModelService
    {
        private readonly IWishCardQueryService _wishCardQueryService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<RoomImage> _roomImgRepository; 
        private readonly int _memberId;
        public WishViewModelService(IWishCardQueryService wishCardQueryService, UserService userService, IRepository<Order> orderRepository, IRepository<Room> roomRepository, IRepository<RoomImage> roomImgRepository)
        {
            _wishCardQueryService = wishCardQueryService;
            _memberId = userService.GetMemberId() ?? 0;
            _orderRepository = orderRepository;
            _roomRepository = roomRepository;
            _roomImgRepository = roomImgRepository;
        }
        // Wishlist
        public WishlistViewModel GetCardList()
        {

            var viewModel = new WishlistViewModel
            {
                CustomCategoryRooms = GetCustomerCategoryRooms()
            };
            return viewModel;
        }

        public List<CustomCategory> GetCustomerCategoryRooms()
        {
            return _wishCardQueryService.GetCardList(_memberId).Select(w => new CustomCategory
            {
                Id = w.Id,
                ImgUrl = w.WishlistDetails.FirstOrDefault()?
                                                .Room.RoomImages.FirstOrDefault()?
                                                .ImageUrl ?? "https://picsum.photos/600/600/?random=10",
                WishlistName = w.WishlistName,
                WishlistURL = $"/Wish/WishlistDetail/{w.Id}",
                SavedCount = w.WishlistDetails.Count
            }).ToList();
        }

        // WishlistDetail
        public WishlistDetailViewModel GetRooms(int wishlistId, int myCount)
        {
            var wishlistDetails = _wishCardQueryService.GetRooms(wishlistId);
            
           var temp = wishlistDetails.Where(g=>g.Room.GuestCount >= myCount);

            var viewModel = new WishlistDetailViewModel
            {
                Id = wishlistId, 
                WishlistName = wishlistDetails.FirstOrDefault()?.Wishlist?.WishlistName,
                Rooms = temp.Select(detail => new RoomCard
                {
                    GuestCount = detail.Room.GuestCount,
                    WishlistDetailId = detail.Id,
                    RoomName = detail.Room.RoomName,
                    RoomId = detail.RoomId,
                    RoomCategory = detail.Room.RoomCategory.RoomCategory1,
                    Price = detail.Room.FixedPrice,
                    Link =$"/Rooms/{detail.RoomId}",
                    ImgUrls = detail.Room.RoomImages.Select(ri => ri.ImageUrl).ToList(),
                    CategoryName = detail.Room.RoomCategory.RoomCategory1,
                    Review = detail.Room.Review,
                    Latitude = detail.Room.Latitude ?? "23.97565",
                    Longitude = detail.Room.Longitude ?? "120.9738819",
                }).ToList()
            };

            return viewModel;
        }

        // Receipt
        public async Task<ReceiptViewModel> GetReceipt(int orderId)
        {
            var order = await _wishCardQueryService.GetReceipt(orderId);
            if (order == null)
            {
                return null;
            }
            var paymentType = order.Ecpays.FirstOrDefault()?.PaymentType ?? "";
            var customerName = $"{order.Member.LastName ?? ""}{order.Member.FirstName}";
            var landlordName = $"{order.Room.Member.LastName ?? ""}{order.Room.Member.FirstName}";
            var receiptViewModel = new ReceiptViewModel
            {
                OrderId = order.Id,
                LandlordName = landlordName,
                PaymentType = paymentType,
                CustomerCount = order.CustomerCount,
                BedCount = order.Room.BedCount,
                Address = $"【{order.Room.PostalCode}】 {order.Room.CountryName} {order.Room.CityName} {order.Room.DistrictName} {order.Room.Street}",
                RoomName = order.Room.RoomName,
                CreatedAt = order.CreatedAt,
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,
                TotalPrice = order.TotalPrice,
                OrderNo = order.OrderNo,
                ReceiptNo = order.ReceiptNo,
                RoomCategory1 = order.Room.CountryName,
                CustomerName = customerName,
                ImageUrl = order.Room.RoomImages.FirstOrDefault()?.Room.RoomImages.FirstOrDefault()?
                                                .ImageUrl ?? "https://picsum.photos/600/600/?random=10"
            };

            return receiptViewModel;
        }


        // MainDetails
        public OrderDetailsViewModel GetOrderRoomInfo(int orderId)
        {
            var order = _orderRepository.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }
            var room = _roomRepository.FirstOrDefault(r=> r.Id == order.RoomId);
            if(room == null)
            {
                return null;

            }
            var imgUrls = _roomImgRepository.List(m=> m.RoomId == room.Id && m.IsDelete == false).Select(m=> m.ImageUrl).ToList();


            var orderViewModel = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                GuestCount = room.GuestCount,
                Title = room.RoomName,
                ImgUrls = imgUrls.Any() ? imgUrls : new List<string> { "https://picsum.photos/600/600/?random=10" },
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,
                Address = $"【{room.PostalCode}】 {room.CountryName} {room.CityName} {room.DistrictName} {room.Street}",
                CustomerCount = order.CustomerCount,
                PriceTotal = order.TotalPrice,
                OrderNo = order.OrderNo,
                ReceiptNo = order.ReceiptNo,
                Latitude = room.Latitude?? "23.97565",
                Longitude = room.Longitude?? "120.9738819",
                RoomId = room.Id,
                RoomLink = $"/Rooms/{room.Id}"
            };

            return orderViewModel;
        }

        // Travel
        public TravelViewModel GetTravelCard(int memberId)
        {
            var orders = _wishCardQueryService.GetTravelCard(memberId);

            var viewModel = new TravelViewModel
            {
                UpcomingBookings = new List<TravelCard>(),
                PendingBookings = new List<TravelCard>(),
                CancelledBookings = new List<TravelCard>()
            };

            foreach (var order in orders)
            {
                // 檢查訂單狀態是否為 4 或 5，如果是則不顯示
                if (order.OrderStatus == 4 || order.OrderStatus == 5)
                {
                    continue;
                }

                var travelCard = new TravelCard
                {
                    Link = $"/Order/MainDetails/{order.Id}",
                    Title = order.Room.RoomName,
                    CategoryName = order.Room.RoomCategory.RoomCategory1,
                    CheckIn = order.CheckIn,
                    CheckOut = order.CheckOut,
                    HostName = $"{order.Room.Member.LastName ?? ""}{order.Room.Member.FirstName}"
                };

                var status = MapOrderStatusToViewModelStatus(order.OrderStatus);

                switch (status)
                {
                    case TravelViewModelStatus.PendingBookings:
                        travelCard.ImgUrl = order.Room.RoomImages.FirstOrDefault()?.ImageUrl ?? "https://picsum.photos/600/600/?random=10";
                        viewModel.PendingBookings.Add(travelCard);
                        break;
                    case TravelViewModelStatus.UpcomingBookings:
                        travelCard.ImgUrl = order.Room.RoomImages.FirstOrDefault()?.ImageUrl ?? "https://picsum.photos/600/600/?random=10";
                        viewModel.UpcomingBookings.Add(travelCard);
                        break;
                    case TravelViewModelStatus.CancelledBookings:
                        travelCard.ImgUrl = order.Room.RoomImages.FirstOrDefault()?.ImageUrl ?? "https://picsum.photos/600/600/?random=10";
                        viewModel.CancelledBookings.Add(travelCard);
                        break;
                }

            }

            return viewModel;
        }

        private TravelViewModelStatus MapOrderStatusToViewModelStatus(int orderStatus)
        {
            switch (orderStatus)
            {
                case 0: // Pending
                    return TravelViewModelStatus.PendingBookings;
                case 1: // Active
                    return TravelViewModelStatus.UpcomingBookings;
                case 2: // Expired
                case 3: // Canceled
                    return TravelViewModelStatus.CancelledBookings;
                default:
                    return TravelViewModelStatus.CancelledBookings;
            }
        }

        public TravelViewModel GetTravelCard()
        {
            return GetTravelCard(_memberId);
        }

        public enum TravelViewModelStatus
        {
            PendingBookings,
            UpcomingBookings,
            CancelledBookings
        }
    }
}
