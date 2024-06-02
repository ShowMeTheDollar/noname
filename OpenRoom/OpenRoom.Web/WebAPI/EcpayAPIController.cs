using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;


namespace OpenRoom.Web.WebAPI
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EcpayAPIController : ControllerBase
    {
        private readonly OpenRoomContext _db;
        private readonly EcpayService _ecpayService;

        public EcpayAPIController(OpenRoomContext db, EcpayService ecpayService)
        {
            _db = db;
            _ecpayService = ecpayService;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveDataFromEcpay([FromForm] IFormCollection transactionResult)
        {
            //步驟: 1.接收綠界回傳的資料 2.比對檢查碼是否正確 3.存資料庫 4.回傳接收成功或失敗訊息給綠界

            //1.接收綠界回傳的資料
            var ecpayData = _ecpayService.ParseDataFromEcpay(transactionResult);
      
            try
            {
                //2.比對檢查碼是否正確
                //var myMacValue = _ecpayService.GetCheckMacValue(ecpayData);

                //if (myMacValue == ecpayData["CheckMacValue"])
                //{
                //正確: 3.存資料庫 4.回傳成功給綠界
                await UpdateOrder(ecpayData);
                return Ok("1|OK");
                //}
                //else
                //{
                //    //錯誤: 4.回傳失敗給綠界
                //    return ResponseError();
                //}
            }
            catch (Exception e)
            {
                return Ok();
            }
        }

        private async Task UpdateOrder(Dictionary<string, string> response)
        {
            var ecpayRecord = await _db.Ecpays.FirstOrDefaultAsync(r => r.MerchantTradeNo == response["MerchantTradeNo"]);
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == ecpayRecord.OrderId);

            if (ecpayRecord == null)
            {
                throw new Exception("Ecpay record not found.");
            }
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            ecpayRecord.RtnCode = Convert.ToInt32(response["RtnCode"]);
            ecpayRecord.RtnMsg = response["RtnMsg"];
            ecpayRecord.TradeNo = response["TradeNo"];
            ecpayRecord.TradeAmt = Convert.ToInt32(response["TradeAmt"]);
            ecpayRecord.PaymentDate = DateTime.ParseExact(response["PaymentDate"], "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(); ;
            ecpayRecord.PaymentType = response["PaymentType"];
            ecpayRecord.PaymentTypeChargeFee = response["PaymentTypeChargeFee"];
            ecpayRecord.TradeDate = DateTime.ParseExact(response["TradeDate"], "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime(); ;
            ecpayRecord.SimulatePaid = Convert.ToInt32(response["SimulatePaid"]);

            order.OrderStatus = (int)(ecpayRecord.RtnCode == 1 ? ecpayRecord.RtnCode : order.OrderStatus);
            await _db.SaveChangesAsync();
        }
    }
}
