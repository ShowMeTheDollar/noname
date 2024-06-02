namespace OpenRoom.Web.ViewModels
{
#nullable disable
    public class RoomDetailViewModel
    {
        public int RoomId { get; set; }
        public string RoomCategoryName { get; set; }
        public int CustomerId { get; set; }
        public string RoomName { get; set; }
        public List<string> RoomImgUrls { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string Street { get; set; }
        public int BedroomCount { get; set; }
        public int BedCount { get; set; }
        public int BathroomCount { get; set; }
        public int GuestCount { get; set; }
        public string LandlordName { get; set; }
        public int LandlordId { get; set; }
        public string AvatarUrl { get; set; }
        public string SelfIntroduction { get; set; }
        public string Job { get; set; }
        public string Live { get; set; }
        public string Obsession { get; set; }
        public string Pet { get; set; }
        public string Language { get; set; }
        public string RoomDescription { get; set; }
        public List<string> AmentityName { get; set; }
        public List<string> AmentityIcon { get; set; }
        public List<int> AmentityTypeId {  get; set; }
        public List<DateTime> CheckIn { get; set; }
        public List<DateTime> CheckOut { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal FixedPrice { get; set; }
        public List<ReviewCardViewModel> ReviewCards { get; set; }
        public string NearyByTrasportation { get; set; }
        public string LocationDesription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double? Rating { get; set; }
        public List<CustomCategory> CustomCategoryRooms { get; set; }
    }
}
