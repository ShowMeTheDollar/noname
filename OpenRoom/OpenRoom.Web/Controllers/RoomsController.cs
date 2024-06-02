using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using OpenRoom.Web.Interfaces;
using OpenRoom.Web.Services;
using OpenRoom.Web.ViewModels;
using System.Xml.Linq;

namespace OpenRoom.Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomsViewModelService _roomsViewModelService;
        public RoomsController(IRoomsViewModelService roomsViewModelService)
        {
            _roomsViewModelService = roomsViewModelService;
        }

        
        [Route("Rooms/{id?}")]
        public async Task<IActionResult> Index(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            RoomDetailViewModel roomDetailViewModel = await _roomsViewModelService.GetRoomDetailsViewModel(id);

            return roomDetailViewModel == null ? RedirectToAction("Index", "Home") : View(roomDetailViewModel);
        }

    }
}
