using ApplicationCore.Interfaces;
using ApplicationCore.Services.RoomListingService;
using ApplicationCore.Services.RoomListingService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenRoom.Web.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomListingAPIController : ControllerBase
    {
        private readonly IRoomDetailsService _roomDetailsService;
        public RoomListingAPIController(IRoomDetailsService roomDetailsService)
        {
            _roomDetailsService = roomDetailsService;
        }

        [HttpPost]
        public async Task<ActionResult<Dictionary<DateTime, decimal?>>> roomPricesApi([FromBody] RoomPricesDto bookingDates)
        {
            return await _roomDetailsService.SearchPricePerNight(bookingDates);
        }
    }
}
