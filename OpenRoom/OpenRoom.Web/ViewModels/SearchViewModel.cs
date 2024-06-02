namespace OpenRoom.Web.ViewModels
{
#nullable disable
    public class SearchViewModel
    {        
        public List<SearchRoomItem> SearchRoomItems { get; set; }
		public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
    public class SearchRoomItem
    {
        public int Id { get; set; }  //這是roomId
        public string RoomName { get; set; }
        public decimal FixedPrice { get; set; }
        public List<string> ImgUrls { get; set; }  //來自RoomImages
		public string RoomCategory { get; set; }  //來自RoomCategory
		public double? Review { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
	}
}
