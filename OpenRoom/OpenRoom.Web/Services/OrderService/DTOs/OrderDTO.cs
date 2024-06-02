namespace OpenRoom.Web.Services.OrderService.DTOs
{
    public class OrderDTO
    {
        public string CheckIn { get; set; }
        public string CheckOut { get; set; } 
        public int GuestCount { get; set; } 
        public int RoomId { get; set; }
        public string RoomCategory { get; set; } 
        public string RoomName { get; set; } 
        public string RoomAddress { get; set; } //地址全名，例如：400台灣台中市中區三民路二段149巷8號
        public string RoomImg { get; set; } //第一張橫的照片

        //public decimal Rating { get; set; } //評價分數
        //public int ReviewCount { get; set; } //評價總則數
      
        public string PricePerNight { get; set; } 
        public decimal PriceTotal { get; set; } 
        public int GuestCountLimitaion { get; set; }

        public string notAvailableCheckIn { get; set; }
        public string notAvailableCheckOut { get; set; }
    }
}
