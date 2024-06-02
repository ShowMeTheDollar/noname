using ApplicationCore.Extensions;
using OpenRoom.Web.ViewModels;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace OpenRoom.Web.Services
{
    public class EcpayService
    {
        private readonly IConfiguration _configuration;

        public EcpayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCheckMacValue(Dictionary<string, string> order)
        {
            //if (order.ContainsKey("CheckMacValue"))
            //{
            //    order.Remove("CheckMacValue");
            //}

            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            var hashKey = _configuration["EcpaySettings:HashKey_Stage"];

            var hashIV = _configuration["EcpaySettings:HashIV_Stage"]; ;

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={hashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower().ToSHA256();

            return checkValue.ToUpper();
        }

        /// <summary>
        /// 將綠界付款頁面所需的參數組合成 Dictionary<>
        /// </summary>
        /// <param name="merchantTradeNo">特店訂單編號</param>
        /// <param name="merchantTradeDate">特店交易時間</param>
        /// <param name="totalPrice">交易金額</param>
        /// <returns></returns>
        public Dictionary<string, string> GenerateDataForEcpay(string merchantTradeNo, string merchantTradeDate, string totalPrice)
        {
            var order = new Dictionary<string, string>
            {
                //特店編號
                { "MerchantID", _configuration["EcpaySettings:MerchantID_Stage"]},

                //特店交易編號
                { "MerchantTradeNo", merchantTradeNo},

                //特店交易時間 yyyy/MM/dd HH:mm:ss
                { "MerchantTradeDate", merchantTradeDate},

                //交易金額
                { "TotalAmount", totalPrice},

                //交易描述
                { "TradeDesc",  "無"},

                //商品名稱
                { "ItemName",  "測試商品"},

                //允許繳費有效天數(付款方式為 ATM 時，需設定此值)
                { "ExpireDate",  "3"},

                //自訂名稱欄位1
                { "CustomField1",  ""},

                //自訂名稱欄位2
                { "CustomField2",  ""},

                //自訂名稱欄位3
                { "CustomField3",  ""},

                //自訂名稱欄位4
                { "CustomField4",  ""},

                //綠界回傳付款資訊的至 此URL
                //{ "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
                //{ "ReturnURL",  $"{website}/EcpayAPI/ReceiveDataFromEcpay"},
                { "ReturnURL",  _configuration["EcpaySettings:ReturnURL"]},
                

                //使用者於綠界 付款完成後，綠界將會轉址至 此URL
                //{ "OrderResultURL", $"{website}/Book/Confirmation"},
                { "OrderResultURL",_configuration["EcpaySettings:OrderResultURL"]},

                //Client端返回特店的按鈕連結
                {"ClientBackURL", _configuration["EcpaySettings:Website"]},

                //忽略付款方式
                { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},

                //交易類型 固定填入 aio
                { "PaymentType",  "aio"},

                //選擇預設付款方式 固定填入 ALL
                { "ChoosePayment",  "ALL"},

                //CheckMacValue 加密類型 固定填入 1 (SHA256)
                { "EncryptType",  "1"},
            };

            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);

            return order;
        }

        public Dictionary<string, string> ParseDataFromEcpay(IFormCollection transactionResult)
        {
            var ecpayData = new Dictionary<string, string>();
            foreach (var key in transactionResult.Keys)
            {
                if (!string.IsNullOrEmpty(transactionResult[key]))
                {
                    ecpayData.Add(key, transactionResult[key]);
                }
            }
            return ecpayData;
        }
    }
}
