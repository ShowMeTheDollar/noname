using Microsoft.AspNetCore.Mvc;
using OpenRoom.Web.Models;
using OpenRoom.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SelectPdf;
using ApplicationCore.DTO;

namespace OpenRoom.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IWishViewModelService _wishViewModelService;
        private readonly UserService _userService;
        public OrderController(IWishViewModelService service, UserService userService)
        {
            _wishViewModelService = service;
            _userService = userService;
        }
        public IActionResult MainDetails(int id)
        {
            var orderRoomInfo = _wishViewModelService.GetOrderRoomInfo(id);
            if(orderRoomInfo == null)
            {
                return Content("查無此訂單!");
            }
            return View(orderRoomInfo);
        }

        public IActionResult MainAmend()
        {
            return View();
        }
        public IActionResult Modify()
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }

        public async Task<IActionResult> Receipt(int id)
        {
            var receipt = await _wishViewModelService.GetReceipt(id);
            if (receipt == null)
            {
                return Content("查無此收據!");
            }
            return View(receipt);
        }

        //[HttpPost]
        //public IActionResult GeneratePdf([FromBody] PdfRequestDto request)
        //{
        //    string html = request.Html.Replace("StrTag", "<").Replace("EndTag", ">");

        //    HtmlToPdf oHtmlToPdf = new HtmlToPdf();
        //    PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
        //    byte[] pdf = oPdfDocument.Save();
        //    oPdfDocument.Close();

        //    return File(
        //       pdf,
        //       "application/pdf",
        //       "Receipt.pdf"
        //    );
        //}


        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Travel()
        {
            var cardViewModel = _wishViewModelService.GetTravelCard();
            
            return View(cardViewModel);
        }
    }
}
