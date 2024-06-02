using ApplicationCore.Entities;

namespace OpenRoom.Web.ViewModels.Hosting
{
    public class CalendarViewModel
    {
        public int Id { get; set; }
        public List<MemberRoom>? Rooms { get; set; }
    }
    
    public class MemberRoom
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal? WeekendPrice { get; set; }
        public decimal? SeparatePrice { get; set; }
        public List<IndividualPrice>? IndividualPrices { get; set; }
    }

    public class IndividualPrice
    {
        public DateTime Date { get; set; }
        public decimal SeparatePrice { get; set; }
    }
}
