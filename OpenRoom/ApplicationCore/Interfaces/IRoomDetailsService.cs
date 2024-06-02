using ApplicationCore.Services.RoomListingService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomDetailsService
    {
        Task<Dictionary<DateTime, decimal?>> SearchPricePerNight(RoomPricesDto roomPrices);
    }
}
