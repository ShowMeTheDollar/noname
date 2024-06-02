using System.Data.SqlTypes;

namespace OpenRoom.Web.ViewModels.Hosting
{
#nullable disable
    public class CreateRoomViewModel
    {
        public int Id { get; set; }
        public string RoomCategory {  get; set; }
        public string Country {  get; set; }
        public string Street {  get; set; }
        public string City { get; set; }
        public string District {  get; set; }
        public string PostalCode {  get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int GuestCount { get; set; }
        public int BedroomCount { get; set; }
        public int BedCount { get; set; }
        public int BathroomCount { get; set; }
        // 使用 List<string> 來接收複選的設施列表
        public List<string> RoomAmenities { get; set; }

        // 使用 List<string> 來接收複選的圖片 URL 列表
        public List<string> ImageURLs { get; set; }

        // 構造函數中初始化列表，確保它們不為 null
        public CreateRoomViewModel()
        {
            RoomAmenities = new List<string>();
            ImageURLs = new List<string>();
        }

        public string RoomName { get; set; }
        public string RoomDescription { get; set; }
        public decimal Price {  get; set; }
    }
    
    
}
