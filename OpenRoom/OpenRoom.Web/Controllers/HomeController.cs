using Infrastructure.Services;
using System.Diagnostics;

namespace OpenRoom.Web.Controllers
{
	public class HomeController : Controller
    {
		private readonly ISearchViewModelService _searchViewModelService;

		public HomeController(ISearchViewModelService searchViewModelService)
		{			
			_searchViewModelService = searchViewModelService;
		}

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Index(int page = 1)
		{
			var cardViewModel = await _searchViewModelService.GetAllRoomsAsync(page);
			return View(cardViewModel);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
