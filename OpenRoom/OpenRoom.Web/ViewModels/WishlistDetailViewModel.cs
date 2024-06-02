

namespace OpenRoom.Web.ViewModels
{
#nullable disable
    public class WishlistDetailViewModel
    {
        public int? Id { get; set; }
        public string WishlistName {  get; set; }
        public List<RoomCard> Rooms { get; set; }
    }
    public class RoomCard
    {
        public int GuestCount { get; set; }
        public int WishlistDetailId {  get; set; } 
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string RoomCategory { get; set; }
        public decimal Price { get; set; }
        public string Link { get; set; }
        public List<string> ImgUrls { get; set; }
        public string CategoryName { get; set; }
        public double? Review { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
    
}
