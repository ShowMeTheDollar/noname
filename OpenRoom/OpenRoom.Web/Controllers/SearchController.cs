using ApplicationCore.DTO;
using Infrastructure.Services;

namespace OpenRoom.Web.Controllers
{
	public class SearchController : Controller
    {
		private readonly SearchViewModelService _searchViewModelService;
		
		public SearchController(SearchViewModelService searchViewModelService)
		{
			_searchViewModelService = searchViewModelService;
		}

		public async Task<IActionResult> Index(int page = 1)
        {
			var viewModel = await _searchViewModelService.GetAllRoomsAsync(page);
			return View(viewModel);
        }

		public async Task<IActionResult> BasicSearch(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory, int page = 1)
		{
			var viewModel = await _searchViewModelService.BasicSearchRoomsAsync(location, checkInDate, checkOutDate, numberOfGuests, roomCategory, page);
			return View("Index", viewModel);
		}

		public async Task<IActionResult> DetailedSearch([FromQuery] SearchFilterDto filter, int page = 1)
		{
			var viewModel = await _searchViewModelService.DetailedSearchRoomsAsync(filter, page);
			return View("Index", viewModel);
		}
	}
}
