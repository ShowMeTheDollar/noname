namespace OpenRoom.Web.ViewModels
{
    #nullable disable
    public class UserProfileViewModel
    {
        public string LandlordName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SelfIntroduction { get; set; }
        public string Job { get; set; }
        public string Live { get; set; }
        public string Obsession { get; set; }
        public string Pet { get; set; }
        public string Language { get; set; }
        public List<ReviewCardViewModel> ReviewCards { get; set; }
        public List<RoomCardViewModel> RoomCards { get; set; }
    }

    public class RoomCardViewModel
    {
        public int RoomId { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string CategoryName { get; set; }
        public double? Rating { get; set; }
    }
}
