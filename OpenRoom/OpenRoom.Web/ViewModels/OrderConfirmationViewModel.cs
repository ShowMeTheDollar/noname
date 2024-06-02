using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
    public class OrderConfirmationViewModel
    {
        // Order Related
        [Display(Name = "訂單編號")]
        public string OrderNo { get; set; }

        //[Display(Name = "訂單狀態")]
        //public int OrderStatusId { get; set; }

        [Display(Name = "入住房源編號")]
        public int RoomId { get; set; }

        [Display(Name = "入住日期")]
        public DateTime CheckIn { get; set; }

        [Display(Name = "退房日期")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "入住人數")]
        public int OrderGuestCount { get; set; }

        [Display(Name = "總計")]
        public decimal TotalPrice { get; set; }
        
        //Room Related
        public string RoomName { get; set; }
        public string RoomImg { get; set; }
        public string RoomAddress { get; set; }

        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        //public int GuestCountLimitation { get; set; }
        public int GuestCount { get; set; }

        //Host Related
        public string HostImg { get; set; }
        public string HostName { get; set; }

        //Customer Related
        public string GuestEmail { get; set;}
    }
}
