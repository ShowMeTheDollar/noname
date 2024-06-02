using ApplicationCore.Entities;
using Infrastructure.Data;

namespace OpenRoom.Web.Controllers
{
    public class EcpayController : Controller
    {

        private readonly OpenRoomContext _db;
        private readonly EcpayService _ecpayService;
        private readonly UserService _userService;

        public EcpayController(OpenRoomContext db, EcpayService ecpayService, UserService userService)
        {
            _db = db;
            _ecpayService = ecpayService;
            _userService = userService;
        }

        /// <summary>
        /// 組合訂單資料送去綠界
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var merchantTradeNo = TempData["MerchantTradeNo"] as string;
            var merchantTradeDate = TempData["MerchantTradeDate"] as string;
            var totalPrice = TempData["TotalPrice"] as string;

            return View(_ecpayService.GenerateDataForEcpay(merchantTradeNo, merchantTradeDate, totalPrice));
        }

        /// <summary>
        /// 使用者按下單鈕,將訂單存到 DB 的 Orders 與 Ecpays
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveOrder(OrderingViewModel orderVM)
        {
            if (TempData["OrderData"] != null)
            {
                orderVM = JsonConvert.DeserializeObject<OrderingViewModel>($"{TempData["OrderData"]}");
                TempData.Remove("OrderData");
            }
            var newOrder = new Order
            {
                RoomId = orderVM.RoomId,
                CheckIn = orderVM.CheckIn.ToUniversalTime(),
                CheckOut = orderVM.CheckOut.ToUniversalTime(),
                CustomerCount = orderVM.GuestCount,
                PaymentMethod = orderVM.PaymentMethod,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                MemberId = (int)_userService.GetMemberId(),
                ReceiptNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20),
                OrderNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20),
                OrderStatus = 0,
                TotalPrice = Math.Round(orderVM.PriceTotal, 0, MidpointRounding.AwayFromZero)
            };

            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();

            var newEcpay = new Ecpay
            {
                OrderId = newOrder.Id,
                MerchantTradeNo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20),
                TradeDate = newOrder.CreatedAt,
            };

            _db.Ecpays.Add(newEcpay);
            await _db.SaveChangesAsync();

            TempData["MerchantTradeNo"] = newEcpay.MerchantTradeNo.ToString();
            TempData["TotalPrice"] = newOrder.TotalPrice.ToString();
            TempData["MerchantTradeDate"] = newOrder.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SubmitOrder(OrderingViewModel orderVM)
        {
            bool isRoomAvailable = CheckInventory(orderVM);

            if (!isRoomAvailable)
            {
                return View("OverlappingOrder");
            }

            if (!User.Identity.IsAuthenticated)
            {
                TempData["OrderData"] = JsonConvert.SerializeObject(orderVM);
                return RedirectToAction("Index", "Login");
            }

            return RedirectToAction("SaveOrder", orderVM);
        }
        /// <summary>
        /// Return true : The room is available. Return false : The room is not available.
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        public bool CheckInventory(OrderingViewModel orderVM)
        {
            var roomID = orderVM.RoomId;
            var checkInDate = orderVM.CheckIn.ToUniversalTime();
            var checkOutDate = orderVM.CheckOut.ToUniversalTime();
            bool isOverlapping = _db.Orders.Any(o => o.RoomId == roomID && 
                                                    (( checkInDate < o.CheckOut && checkOutDate > o.CheckIn) 
                                                       || (checkInDate == o.CheckIn && checkOutDate == o.CheckOut)
                                                    ));

            bool isRoomAvailable = !isOverlapping;
            return isRoomAvailable;
        }
    }
}
