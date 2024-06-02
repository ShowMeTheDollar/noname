using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using OpenRoom.Web.Views.Hosting;
using System.Globalization;

namespace OpenRoom.Web.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostingApiController : ControllerBase
    {        
        private readonly ICreateRoomService _createRoomService;
        private readonly UserService _userService;
        private readonly OpenRoomContext _context;
        private readonly IRepository<Room> _roomRepo;
        public HostingApiController(ICreateRoomService createRoomService, UserService userService, OpenRoomContext context = null, IRepository<Room> roomRepo = null)
        {
            _createRoomService = createRoomService;
            _userService = userService;
            _context = context;
            _roomRepo = roomRepo;
        }

        [HttpPost("CreateRoomApi")]
        public async Task<IActionResult> CreateRoomApi([FromBody] CreateRoomDto createRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var memberId = _userService.GetMemberId();
                var memberName = _userService.GetMemberName();
                if (memberId == null)
                {
                    return Unauthorized("User must be logged in to create a room.");
                }
                createRoomDto.MemberId = (int)memberId; // MemberId is needed and it's an int
                var roomId = await _createRoomService.SaveRoomAsync(createRoomDto);             

                string redirectUrl = Url.Action("Source", "Hosting", new { id = createRoomDto.MemberId }); // Use memberId from your DTO or context//此行碼會生成一個絕對 URL，並且協議（HTTP 或 HTTPS）會與當前請求的協議相匹配。(在控制器中構建 URL 並想要保持與當前請求相同的協議)
                
                HttpContext.Response.Cookies.Delete("formData");//清除cookie
                return Ok(new { success = true, redirectUrl = redirectUrl, memberName = memberName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        //展示現有照片
        // GET: api/HostingApi/GetImages/{id}
        [HttpGet("GetImages/{id}")]
        public async Task<ActionResult<IEnumerable<string>>> GetImages(int id)
        {
            var images = await _context.RoomImages
                .Where(ri => ri.RoomId == id)
                .Select(ri => ri.ImageUrl)
                .ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        //更新照片
        // PUT: api/HostingApi/UpdateImages/{id}
        [HttpPut("UpdateImages/{id}")]
        public async Task<IActionResult> UpdateImages(int id, [FromBody] List<string> newImageUrls)
        {
            var roomImages = await _context.RoomImages
                .Where(ri => ri.RoomId == id)
                .ToListAsync();

            if (roomImages == null)
            {
                return NotFound();
            }

            // 移除舊的照片URLs
            _context.RoomImages.RemoveRange(roomImages);

            // 新增新的照片URLs
            foreach (var url in newImageUrls)
            {
                _context.RoomImages.Add(new RoomImage { RoomId = id, ImageUrl = url });
            }

            await _context.SaveChangesAsync();

            return NoContent(); // 或返回適當的響應
        }

        [HttpPost("UpdateCalendarPrice")]
        public async Task<IActionResult> UpdateCalendarPrice([FromForm] CalendarPriceDto updatePrice)
        {
            Room room = _context.Rooms.First(r => r.Id == updatePrice.RoomId);
            room.FixedPrice = updatePrice.FixedPrice;
            room.WeekendPrice = updatePrice.WeekendPrice;
            //TimeSpan differTime = DateTime.Now - DateTime.UtcNow;
            var pickedDates = HttpContext.Request.Cookies["PickedDates"];
            if (!string.IsNullOrEmpty(pickedDates) && updatePrice.SeparatePrice != null)
            {
                HttpContext.Response.Cookies.Delete("PickedDates");
                var dates = pickedDates.Split(',');
                foreach (var dateStr in dates)
                {
                    DateTime date;

                    if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite, out date))
                    {
                        //date -= differTime;
                        date = date.AddHours(-8);
                        var existingPrice = _context.RoomPrices.FirstOrDefault(rp => rp.RoomId == room.Id && rp.Date == date);
                        if (existingPrice != null)
                        {
                            // 如果找到了，則更新價格
                            existingPrice.SeparatePrice = (decimal)updatePrice.SeparatePrice;
                            _context.RoomPrices.Update(existingPrice);
                        }
                        else
                        {
                            // 如果沒有找到，則新增一個新的價格記錄
                            var roomPrice = new RoomPrice
                            {
                                RoomId = room.Id,
                                Date = date,
                                SeparatePrice = (decimal)updatePrice.SeparatePrice
                            };
                            _context.RoomPrices.Add(roomPrice);
                        }
                    }
                }
            }

            try
            {
                _roomRepo.Update(room);
                return RedirectToAction("Calendar", "Hosting", new { roomid = room.Id });   
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("GetRoomInfo")]
        public ActionResult<IEnumerable<RoomInfo>> GetRoomInfo()
        {
            var rooms = _context.Rooms
                .Where(room => room.RoomStatus == 1 && room.IsDelete == false)
                .Select(room => new RoomInfo
                {
                    RoomId = room.Id,
                    RoomName = room.RoomName,
                    CountryName = room.CountryName,
                    CityName = room.CityName,
                    DistrictName = room.DistrictName,
                    FixedPrice = room.FixedPrice
                })
                .ToList();

            return Ok(rooms);
        }
    }
}
