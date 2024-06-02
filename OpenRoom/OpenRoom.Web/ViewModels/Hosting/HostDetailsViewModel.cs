using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenRoom.Web.ViewModels.Hosting
{
#nullable disable
    public class HostDetailsViewModel
    {
        public int RoomId {  get; set; }
        public string Name { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>(); // 初始化確保不為null
        public string Title { get; set; }
        public string Description { get; set; }
        public int GuestNumber { get; set; }
        public int BedroomCount { get; set; }
        public int BedCount { get; set; }
        public int BathroomCount { get; set; }
        //public string State { get; set; }
        public int State { get; set; } // 確保這裡與你表單中 asp-for="State" 的屬性類型一致
        public IEnumerable<SelectListItem> RoomStatusOptions { get; set; }
        //public List<Amentity> Amenities { get; set; }       
        public List<string> Amenities { get; set; }
        public string Category { get; set; }

        public int RoomCategoryId { get; set; } // Assuming the ID is an integer
        public IEnumerable<SelectListItem> RoomCategories { get; set; }

        //public string Address { get; set; }
        public AddressDto Address { get; set; }
        public string StreetDescription { get; set; }
        public string Transportation { get; set; }
        public DateTime CheckInStartTime { get; set; }
        public string CheckInStartTimeString
        {
            get { return CheckInStartTime.ToString("HH:mm"); }
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var time))
                {
                    // 獲取當前日期
                    var currentDate = DateTime.Now.Date;
                    // 結合當前日期和用戶選擇的時間
                    CheckInStartTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, 0);
                }
            }
        }
        public DateTime CheckInEndTime { get; set; }
        public string CheckInEndTimeString
        {
            get { return CheckInEndTime.ToString("HH:mm"); }
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var time))
                {
                    // 獲取當前日期
                    var currentDate = DateTime.Now.Date;
                    // 結合當前日期和用戶選擇的時間
                    CheckInEndTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, 0);
                }
            }
        }
        public DateTime CheckOutTime { get; set; }
        public string CheckOutTimeString
        {
            get { return CheckOutTime.ToString("HH:mm"); }
            set
            {
                if (DateTime.TryParseExact(value, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var time))
                {
                    // 獲取當前日期
                    var currentDate = DateTime.Now.Date;
                    // 結合當前日期和用戶選擇的時間
                    CheckOutTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, time.Hour, time.Minute, 0);
                }
            }
        }

        public DateTime EditedAt { get; set; }

    }
    

    public class AddressDto
    {        
        public string Country { get; set; }     
        public string StreetAddress { get; set; }  
        public string City { get; set; } 
        public string District { get; set; }
        public string PostalCode { get; set; }
    }
}