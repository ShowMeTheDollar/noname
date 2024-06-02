using NuGet.Protocol.Core.Types;


namespace OpenRoom.Web.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string Title { get; set; }
        public List<string> ImgUrls { get; set; } 
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Address { get; set; }
        public int CustomerCount { get; set; }
        public decimal PriceTotal { get; set; }
        public string OrderNo {  get; set; }
        public int RoomId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string RoomLink {  get; set; }
        public string ReceiptLink {  get; set; }
        public int GuestCount { get; set; }
        public string ReceiptNo { get; set; }
    }
}
