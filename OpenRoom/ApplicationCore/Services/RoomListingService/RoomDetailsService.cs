using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services.RoomListingService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.RoomListingService
{
    public class RoomDetailsService : IRoomDetailsService
    {
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<RoomPrice> _roomPriceRepo;
        public RoomDetailsService(IRepository<Room> roomRepo, IRepository<RoomPrice> roomPriceRepo)
        {
            _roomRepo = roomRepo;
            _roomPriceRepo = roomPriceRepo;
        }

        public async Task<Dictionary<DateTime, decimal?>> SearchPricePerNight(RoomPricesDto roomPrices)
        {
            var room = await _roomRepo.GetByIdAsync(roomPrices.RoomId);
            //CheckOut.Date不用AddDays(1)，因為不需要CheckOut當天的價格
            var separatePrices = _roomPriceRepo.List(rp => rp.RoomId == room.Id && rp.Date.AddHours(8) >= roomPrices.CheckIn.AddHours(8).Date && rp.Date.AddHours(8) < roomPrices.CheckOut.AddHours(8).Date);
            var priceDictionary = new Dictionary<DateTime, decimal?>();

            for (DateTime date = roomPrices.CheckIn.AddHours(8).Date; date < roomPrices.CheckOut.AddHours(8).Date; date = date.AddDays(1))
            {
                var matchingPrice = separatePrices.FirstOrDefault(s => s.Date.AddHours(8).Date == date);
                if (separatePrices.Count > 0 && matchingPrice != null)
                {
                    priceDictionary[date] = matchingPrice.SeparatePrice;
                }
                else if (room.WeekendPrice.HasValue && (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday))
                {
                    priceDictionary[date] = room.WeekendPrice;
                }
                else
                {
                    priceDictionary[date] = room.FixedPrice;
                }               
            }
            
            return priceDictionary;
        }
    }
}
