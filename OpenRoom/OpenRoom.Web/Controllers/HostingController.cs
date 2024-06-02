using Microsoft.AspNetCore.Mvc;
using OpenRoom.Web.Interfaces;
using OpenRoom.Web.ViewModels;
using OpenRoom.Web.ViewModels.Hosting;
using System.Threading.Tasks;
using ApplicationCore.Interfaces; // 假設您的服務層interface在這個命名空間下
using ApplicationCore.Services;
using isRock.LineBot;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;



namespace OpenRoom.Web.Controllers
{
    [Authorize]
    public class HostingController : Controller
    {
        private readonly IHostingSourceViewModelService _hostingSourceService;//改擷取介面換實作
        private readonly IImageFileUploadService _cloudinaryService;
        private readonly ICreateRoomService _createRoomService;
        private readonly UserService _userService;
        private readonly IHostingRoomService _hostingRoomService;
        private readonly OpenRoomContext _context;
        private readonly int _memberId;

        public HostingController(IHostingSourceViewModelService hostingSourceService, IImageFileUploadService cloudinaryService, ICreateRoomService createRoomService, UserService userService, IHostingRoomService hostingRoomService = null, OpenRoomContext context = null)
        {
            _hostingSourceService = hostingSourceService;
            _cloudinaryService = cloudinaryService;
            _createRoomService = createRoomService;
            _userService = userService;
            _hostingRoomService = hostingRoomService;
            _context = context;
            _memberId = userService.GetMemberId()??0;
        }
        public IActionResult Today()
        {                        
            var viewModel = _hostingSourceService.GetReservations(_memberId);
            return View(viewModel);
        }

        public async Task<IActionResult> Calendar(int? roomid)
        {
            var memberId = _userService.GetMemberId();
            CalendarViewModel calendarViewModel = await _hostingSourceService.GetCalendarPrice(memberId.Value);
            if (roomid.HasValue)
            {
                ViewBag.SelectedRoomId = roomid.Value;
            }
            return View(calendarViewModel);
        }

        public IActionResult Details(int roomId)
        {
            var hostDetailsViewModel = _hostingSourceService.GetHostDetails(roomId);
            if (hostDetailsViewModel == null)
            {
                return NotFound();
            }
            return View(hostDetailsViewModel);
        }

        [HttpPost]
        public IActionResult UpdateRoomName(int roomId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                // 可以返回一個錯誤訊息，指出名稱不能為空
                TempData["ErrorMessage"] = "房源名稱不能是空值";
                return RedirectToAction(nameof(Details), new { roomId = roomId });
            }

            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {

                return NotFound();
            }

            room.RoomName = name.Trim(); // 更新房間名稱
            _hostingRoomService.UpdateRoom(room); // 更房源的方法

            TempData["SuccessMessage"] = "房源名稱更新成功！";
            return RedirectToAction(nameof(Details), new { roomId = roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomDescription(int roomId, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                TempData["ErrorMessage"] = "房源描述不能是空值";
                return RedirectToAction(nameof(Details), new { roomId });
            }

            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.RoomDescription = description.Trim();
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "房源描述更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomCounts(int roomId, int guestNumber, int bedroomCount, int bedCount, int bathroomCount)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.GuestCount = guestNumber;
            room.BedroomCount = bedroomCount;
            room.BedCount = bedCount;
            room.BathroomCount = bathroomCount;
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "房源容納人數及相關設施更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomState(int roomId, int state)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.RoomStatus = state; // 假设State是一个枚举或者整型字段
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "房源狀態更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomAmenities(int roomId, List<string> amenities)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            // Clear existing amenities
            //room.RoomAmenities.Clear();

            // First, remove existing RoomAmenities for the room
            var roomAmenitiesToRemove = _context.RoomAmenities.Where(ra => ra.RoomId == roomId).ToList();
            _context.RoomAmenities.RemoveRange(roomAmenitiesToRemove);
            _context.SaveChanges(); // Ensure changes are committed before adding new ones

            // Loop through the list of amenity names
            foreach (var amenityName in amenities)
            {
                // Find the corresponding RoomAmentityCategory by name
                var amenityCategory = _context.RoomAmentityCategories
                                                 .FirstOrDefault(c => c.AmentityName == amenityName);
                // Then, add new amenities
                if (amenityCategory != null)
                {
                    // Create a new RoomAmenity and add it to the room
                    var newRoomAmenity = new RoomAmenity
                    {
                        RoomId = roomId,
                        AmentityId = amenityCategory.Id //  the ID is the link between the category and the amenity
                    };
                    room.RoomAmenities.Add(newRoomAmenity);// Assuming room.RoomAmenities is initialized
                }
            }

            _hostingRoomService.UpdateRoom(room);// This might need to save changes within the method

            TempData["SuccessMessage"] = "房源設施與服務更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomCategory(int roomId, int categoryId)
        {
            // Verify the RoomCategory exists
            var categoryExists = _context.RoomCategories.Any(c => c.Id == categoryId);
            if (!categoryExists)
            {
                TempData["ErrorMessage"] = "指定的房源類別不存在。";
                return RedirectToAction(nameof(Details), new { roomId });
            }
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
            {
                return NotFound();
            }

            // Update the RoomCategoryId FK
            room.RoomCategoryId = categoryId;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "房源類別更新成功！";
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and add logging here)
                TempData["ErrorMessage"] = "更新房源類別時發生錯誤。";
            }


            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomAddress(int roomId, string streetAddress, string city, string country, string district, string postalCode)
        {
            if (string.IsNullOrEmpty(streetAddress) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(district) || string.IsNullOrEmpty(postalCode))
            {
                TempData["ErrorMessage"] = "All fields must be filled out.";
                return RedirectToAction(nameof(Details), new { roomId });
            }
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }
           
            room.Street = streetAddress.Trim();
            room.CityName = city.Trim();
            room.CountryName = country.Trim();
            room.DistrictName = district.Trim();
            room.PostalCode = postalCode.Trim();
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "地址更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateStreetDescription(int roomId, string streetDescription)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.LocationDesription = streetDescription.Trim();
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "街區描述更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomTransportation(int roomId, string transportation)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.NearyByTrasportation = transportation.Trim();
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "周邊交通更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomCheckIn(int roomId, string checkInStartTimeString, string checkInEndTimeString)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.CheckInStartTime = DateTime.ParseExact(checkInStartTimeString, "HH:mm", null);
            room.CheckInEndTime = DateTime.ParseExact(checkInEndTimeString, "HH:mm", null);
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "入住時段更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }

        [HttpPost]
        public IActionResult UpdateRoomCheckOut(int roomId, string checkOutTimeString)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null)
            {
                return NotFound();
            }

            room.CheckOutTime = DateTime.ParseExact(checkOutTimeString, "HH:mm", null);
            _hostingRoomService.UpdateRoom(room);

            TempData["SuccessMessage"] = "退房時間更新成功！";
            return RedirectToAction(nameof(Details), new { roomId });
        }
        public IActionResult Source()
        {
                       
            var sourceViewModel = _hostingSourceService.GetSource(_memberId);
            
            return View(sourceViewModel);
        }
        [HttpPost]
        public IActionResult Delete(int roomId)
        {
            _hostingSourceService.DeleteRoom(roomId);
            return RedirectToAction("Source");
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {

            var viewModel = new HostingViewModel
            {
                Title = "Page 0",
                MemberName = "Vivian"
            };


            return View("CreateRoom", viewModel);
        }


    }
}
