using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using isRock.LineBot;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace OpenRoom.Admin.WebApi
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class RoomSourceListController : ControllerBase
    {
        private readonly OpenRoomContext _context;
       
        public RoomSourceListController(OpenRoomContext context)
        {
            _context = context;
        }
        // GET: api/RoomSourceList
        [HttpGet]
        public async Task<IActionResult> GetRoomSources()
        {
            // Fetch members from the database
            var roomSources = await _context.Rooms.ToListAsync();

            // Transform the data to the DTO
            var roomDto = roomSources.Select(r => new RoomSourceDto
            {
                Id = r.Id,
                RoomName = r.RoomName,
                GuestCount = r.GuestCount,
                BedroomCount = r.BedroomCount,
                BedCount = r.BedCount,
                BathroomCount = r.BathroomCount,
                RoomCategoryID = r.RoomCategoryId,
                MemberID = r.MemberId,
                FixedPrice = r.FixedPrice,
                CountryName = r.CountryName,
                CityName = r.CityName,
                Street = r.Street,
                DistrictName = r.DistrictName,
                PostalCode = r.PostalCode



            }).ToList();

            // Return the DTOs
            return Ok(roomDto);
        }
    }
}
