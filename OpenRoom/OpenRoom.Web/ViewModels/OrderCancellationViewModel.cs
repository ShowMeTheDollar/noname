using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
    
    public class OrderCancellationViewModel
    {
        // Order Related
        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }

        [Display(Name = "訂單狀態")]
        public int OrderStatusId { get; set; }

        [Display(Name = "入住房源編號")]
        public int RoomId { get; set; }

        [Display(Name = "入住日期")]
        public DateTime CheckIn { get; set; }

        [Display(Name = "退房日期")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "總計")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "退款方式")]
        public int PaymentMethod { get; set; }

        //Room Related
        public string RoomName { get; set; }
        public string RoomImg { get; set; }

        //Host Related
        public string HostName { get; set; }
    }
}
