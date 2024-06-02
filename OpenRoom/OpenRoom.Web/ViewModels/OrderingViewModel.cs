using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
    public class OrderingViewModel
    {
        // Order Related

        [Display(Name = "入住房源編號")]
        public int RoomId { get; set; }

        [Display(Name = "入住日期")]
        public DateTime CheckIn { get; set; }

        [Display(Name = "退房日期")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "入住人數")]
        public int GuestCount { get; set; }

        [Display(Name = "總計")]
        public decimal PriceTotal { get; set; }

        [Display(Name = "付款方式")]
        public int PaymentMethod { get; set; }

        //Room Related
        public string RoomName { get; set; }
        public string RoomImg { get; set; }
        public string RoomAddress { get; set; }
        public string RoomCategory { get; set; }
        public Dictionary<string, string> PricePerNight { get; set; }
        public decimal Rating {  get; set; }
        public int ReviewCount { get; set; }

        public int GuestCountLimitation { get; set; }

        public string NotAvailableCheckIn { get; set; }
        public string NotAvailableCheckOut { get; set; }

    }
}
