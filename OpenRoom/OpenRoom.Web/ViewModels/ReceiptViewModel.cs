namespace OpenRoom.Web.ViewModels
{
    public class ReceiptViewModel
    {
        public int OrderId { get; set; }
        public string RoomName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderNo { get; set; }
        public string ReceiptNo { get; set; }
        public string RoomCategory1 { get; set; }
        public string CustomerName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; }
        public int BedCount { get; set; }
        public int CustomerCount { get; set; }
        public string? PaymentType { get; set; }
        public string LandlordName { get; set; }
    }
}
