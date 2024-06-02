using NuGet.Protocol.Core.Types;

namespace OpenRoom.Web.ViewModels
{
#nullable disable
    public class TravelViewModel
    {
        public List<TravelCard> UpcomingBookings { get; set; }
        public List<TravelCard> PendingBookings {  get; set; }
        public List<TravelCard> CancelledBookings { get; set; }
    }
    public class TravelCard
    {
        public string Link { get; set; } 
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string ImgUrl { get; set; }
        public string HostName { get; set; }
    }
}
