namespace OpenRoom.Web.ViewModels
{
#nullable disable
    public class WishlistViewModel
    {
        
        public List<CustomCategory> CustomCategoryRooms { get; set; }
    }
    public class CustomCategory
    {
        public int? Id { get; set; }
        public string ImgUrl { get; set; }
        public string WishlistName { get; set; } 
        public string WishlistURL { get; set; }
        public int? SavedCount { get; set; }
    }
}

