using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using OpenRoom.Web.Services.OrderService;
using OpenRoom.Web.Services.OrderService.DTOs;

namespace OpenRoom.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly OrderService _orderService;
        private readonly EcpayService _ecpayService;    
        private readonly OpenRoomContext _db;
        public BookController(OrderService service, OpenRoomContext db, EcpayService ecpayService)
        {
            _orderService = service;
            _db = db;
            _ecpayService = ecpayService;
        }

        public IActionResult Stays([FromQuery] OrderDTO orderParams) 
        {
            var orderInfo = _orderService.GetOrderInfo(orderParams);
            return View(orderInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmation([FromForm] IFormCollection transactionResult)
        {
            var ecpayData = _ecpayService.ParseDataFromEcpay(transactionResult);

            if (ecpayData["RtnCode"] == "1")
            {
                var merchantTradeNo = ecpayData["MerchantTradeNo"];
                var orderNo = await _db.Ecpays
                                        .Where(record => record.MerchantTradeNo == merchantTradeNo)
                                        .Select(record => record.OrderId)
                                        .SelectMany(orderId => _db.Orders.Where(o => o.Id == orderId).Select(o => o.OrderNo))
                                        .FirstOrDefaultAsync();
                var orderConfirmation = await _orderService.GetOrderConfirmation(orderNo);

                return View(orderConfirmation);
            }
            else
            {
                return View("FailedOrder");
            }

        }

    }
}
