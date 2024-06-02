using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace OpenRoom.Web.ViewModels.Hosting
{
    public class SourceListOrderViewModel
    {
        #nullable disable

        public int OrderId { get; set; }
        public string RoomName { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
        public string MemberName { get; set; }
    }

    public class ReservationViewModel
    {
        public string MemberName { get; set; }
        [Display(Name = "即將入住")]
        public List<SourceListOrderViewModel> UpcomingCheckIns { get; set; } = new List<SourceListOrderViewModel>();

        [Display(Name = "目前接待中")]
        public List<SourceListOrderViewModel> CurrentStays { get; set; } = new List<SourceListOrderViewModel>();

        [Display(Name = "即將退房")]
        public List<SourceListOrderViewModel> UpcomingCheckOuts { get; set; } = new List<SourceListOrderViewModel>();
    }
}
