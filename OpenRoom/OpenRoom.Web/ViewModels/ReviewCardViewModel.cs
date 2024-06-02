namespace OpenRoom.Web.ViewModels
{
    #nullable disable
    public class ReviewCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int rate { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime EditAt { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
    }
}
