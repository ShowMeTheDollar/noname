using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Drawing;
using Elfie.Serialization;
using ApplicationCore.Enums;
using OpenRoom.Web.ViewModels.Hosting;
using System.Text;
using ApplicationCore.Interfaces;
using isRock.LineBot;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using ApplicationCore.Services;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using AddressDto = OpenRoom.Web.ViewModels.Hosting.AddressDto;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace OpenRoom.Web.Services
{
    
    public class HostingSourceViewModelService : IHostingSourceViewModelService
    {
        private readonly IHostingRoomService _hostingRoomService;
        private readonly IHostingRoomOrderService _hostingRoomOrderService;
        private readonly IRoomPriceQueryService _roomPriceQueryService;
        private readonly IHostingQueryService _hostingQueryService;
        private readonly UserService _userService;

        public HostingSourceViewModelService(IHostingRoomService hostingRoomService, IHostingRoomOrderService hostingRoomOrderService, UserService userService = null, IRoomPriceQueryService roomPriceQueryService = null, IHostingQueryService hostingQueryService = null)
        {
            _hostingRoomService = hostingRoomService;
            _hostingRoomOrderService = hostingRoomOrderService;
            _userService = userService;
            _roomPriceQueryService = roomPriceQueryService;
            _hostingQueryService = hostingQueryService;
        }



        //private string GenerateTimeOptions(TimeSpan selectedTime, int startHour = 0, int endHour = 23)
        //{
        //    var optionsHtml = new StringBuilder();
        //    for (int hour = startHour; hour <= endHour; hour++)
        //    {
        //        var isSelected = selectedTime.Hours == hour;
        //        var timeValue = $"{hour:D2}:00"; // Ensuring two-digit hour format
        //        optionsHtml.Append($"<option value='{timeValue}' {(isSelected ? "selected='selected'" : "")}>{timeValue}</option>");
        //    }
        //    return optionsHtml.ToString();
        //}
        //// 處理表單提交的後端方法
        //public void ProcessUserSelection(string selectedTimeOption)
        //{
        //    // 解析前端發送的時間選擇（例如 "14:00"）
        //    TimeSpan selectedTime = TimeSpan.Parse(selectedTimeOption);

        //    // 使用 DateTime.Now 獲取當前日期
        //    DateTime currentDate = DateTime.Now.Date;

        //    // 結合當前日期和用戶選擇的時間
        //    DateTime dateTimeToStore = currentDate.Add(selectedTime);

        //    // 現在你可以將 dateTimeToStore 存入資料庫
        //    // 存入資料庫的代碼依賴於你所使用的數據訪問技術
        //}
        public HostDetailsViewModel GetHostDetails(int roomId)
        {
            var room = _hostingRoomService.GetRoomDetailsById(roomId);
            if (room == null) throw new KeyNotFoundException("Room not found");

            // Fetch all room categories
            var roomCategories = _hostingRoomService.GetAllRoomCategories().Select(rc => new SelectListItem
            {
                Value = rc.Id.ToString(),
                Text = rc.RoomCategory1 //RoomCategory1 holds the category name
            }).ToList();
            //檢查所有時間是否相同
            bool areAllTimesEqual = room.CheckInStartTime.HasValue && room.CheckInEndTime.HasValue && room.CheckOutTime.HasValue &&
                                    room.CheckInStartTime.Value == room.CheckInEndTime.Value &&
                                    room.CheckInStartTime.Value == room.CheckOutTime.Value;

            var hostDetails = new HostDetailsViewModel
            {               
                RoomId = roomId,
                ImageUrls = room.RoomImages.Select(ri => ri.ImageUrl).ToList(),
                Name = room.RoomName,
                RoomCategoryId = room.RoomCategoryId,
                RoomCategories = roomCategories,
                //Category = room.RoomCategory?.RoomCategory1,                
                Description = room.RoomDescription,
                GuestNumber = room.GuestCount,
                BedroomCount = room.BedroomCount,
                BedCount = room.BedCount,
                BathroomCount = room.BathroomCount,
                State = room.RoomStatus, // 假设 room.RoomStatus 是存储状态的 int 属性
                RoomStatusOptions = Enum.GetValues(typeof(RoomStatus)).Cast<RoomStatus>()
                .Select(status => new SelectListItem
                {
                    Text = status.ToString(),
                    Value = ((int)status).ToString(),
                    Selected = status == (RoomStatus)room.RoomStatus
                }),
                Address = new AddressDto
                {
                    Country = room.CountryName,
                    City = room.CityName,
                    StreetAddress = room.Street,
                    District = room.DistrictName,
                    PostalCode = room.PostalCode
                },
                StreetDescription = room.LocationDesription,
                Transportation = room.NearyByTrasportation,
                Amenities = room.RoomAmenities.Select(ra => ra.Amentity?.AmentityName).ToList(),
                //CheckInStartTimeString = room.CheckInStartTime.HasValue? room.CheckInStartTime.Value.ToString("HH:mm"): "14:00",
                //CheckInEndTimeString = room.CheckInEndTime.HasValue? room.CheckInEndTime.Value.ToString("HH:mm"): "22:00",
                //CheckOutTimeString = room.CheckOutTime.HasValue? room.CheckOutTime.Value.ToString("HH:mm"): "12:00"
                
                CheckInStartTimeString = areAllTimesEqual? "14:00" : room.CheckInStartTime.HasValue ? room.CheckInStartTime.Value.ToString("HH:mm")  : "14:00",       
                CheckInEndTimeString = areAllTimesEqual ? "22:00": room.CheckInEndTime.HasValue ? room.CheckInEndTime.Value.ToString("HH:mm") : "22:00",              
                CheckOutTimeString = areAllTimesEqual ? "12:00" : room.CheckOutTime.HasValue ? room.CheckOutTime.Value.ToString("HH:mm")  : "12:00",
                
            };
                    
            return hostDetails;           
        }
        
       

        public ReservationViewModel GetReservations(int memberId)
        {
            
            var orders = _hostingRoomOrderService.GetHostingRoomOrdersByMember(memberId);
            var today = DateTime.Today;

            // 取得UserService 當前登錄用戶的姓名
            var memberName = _userService.GetMemberName();
            var viewModel = new ReservationViewModel
            {
                MemberName = memberName, // 將取到的用戶名字到 ViewModel
                UpcomingCheckIns = GetUpcomingCheckIns(orders, today),
                CurrentStays = GetCurrentStays(orders, today),
                UpcomingCheckOuts = GetUpcomingCheckOuts(orders, today),
            };

            return viewModel;
        }

        private List<SourceListOrderViewModel> GetUpcomingCheckIns(List<Order> orders, DateTime today)
        {
            return orders
                .Where(o => o.CheckIn.Date > today && o.OrderStatus == (int)OrderStatus.Active)
                .OrderBy(o => o.CheckIn)
                .Take(3)
                .Select(o => new SourceListOrderViewModel
                {
                    OrderId = o.Id,
                    RoomName = o.Room.RoomName,
                    GuestName = $"{o.Member.FirstName} {o.Member.LastName}",
                    CheckInDate = o.CheckIn,
                    CheckOutDate = o.CheckOut,
                    Price = (int)o.TotalPrice,
                })
                .ToList();
        }

        private List<SourceListOrderViewModel> GetCurrentStays(List<Order> orders, DateTime today)
        {
            return orders
                .Where(o => o.CheckIn.Date <= today && o.CheckOut.Date >= today && o.OrderStatus == (int)OrderStatus.Ongoing)
                .OrderBy(o => o.CheckIn)
                .Take(3)
                .Select(o => new SourceListOrderViewModel
                {
                    OrderId = o.Id,
                    RoomName = o.Room.RoomName,
                    GuestName = $"{o.Member.FirstName} {o.Member.LastName}",
                    CheckInDate = o.CheckIn,
                    CheckOutDate = o.CheckOut,
                    Price = (int)o.TotalPrice,
                })
                .ToList();
        }

        private List<SourceListOrderViewModel> GetUpcomingCheckOuts(List<Order> orders, DateTime today)
        {
            return orders
                .Where(o => o.CheckOut.Date > today.AddDays(1) && o.CheckOut.Date < today.AddDays(5) && (o.OrderStatus == (int)OrderStatus.Active || o.OrderStatus == (int)OrderStatus.Ongoing))
                .OrderBy(o => o.CheckOut)
                .Take(3)
                .Select(o => new SourceListOrderViewModel
                {
                    OrderId = o.Id,
                    RoomName = o.Room.RoomName,
                    GuestName = $"{o.Member.FirstName} {o.Member.LastName}",
                    CheckInDate = o.CheckIn,
                    CheckOutDate = o.CheckOut,
                    Price = (int)o.TotalPrice,
                })
                .ToList();
        }

        public SourceViewModel GetSource(int hostingId)//id寫這裡，傳參數
        {
            var sources = _hostingRoomService.GetHostingRooms(hostingId);

            var result = new SourceViewModel
            {
                Sources = sources.Select(source => new RoomSource//集合才能select，先where做篩選再select(不一定)
                {
                    Id = source.Id,
                    ImgUrl = source.RoomImages.FirstOrDefault()?.ImageUrl ?? "no image",
                    //ImgUrl = source.RoomImages.Select(img => img.ImageUlr).FirstOrDefault(), // Get the URL of the first image // 獲取第一個圖片的 URL
                    Category = source.RoomCategory?.RoomCategory1 ?? "no category",
                    Name = source.RoomName,
                    Description = source.RoomDescription,
                    State = Enum.GetName(typeof(RoomStatus), source.RoomStatus),
                    CustomerCount = source.GuestCount,
                    BedRoomCount = source.BedroomCount,
                    BedCount = source.BedCount,
                    BathroomCount = source.BathroomCount,
                    District = source.DistrictName,
                    City = source.CityName,
                    Services = source.RoomAmenities.Select(ra => ra.Amentity?.AmentityName ?? "no amentit").ToList(), // 轉換設施為名稱的列表

                }).ToList(),
                //SourceCount = sources.Count().ToString() + "個房源"
                SourceCount = $"{sources.Count}個房源"

            };



            return result;
        }

        public void DeleteRoom(int roomId)
        {
            _hostingRoomService.DeleteRoom(roomId);
        }

        public async Task<SourceViewModel> GetSourceAsync(int hostingId) //非同步用法
        {
            
            var sources = await _hostingRoomService.GetHostingRoomsAsync(hostingId);

            var result = new SourceViewModel
            {
                Sources = sources.Select(source => new RoomSource
                {
                    Id = source.Id,
                    ImgUrl = source.RoomImages.FirstOrDefault()?.ImageUrl ?? "no image",
                    Category = source.RoomCategory?.RoomCategory1 ?? "no category",
                    Name = source.RoomName,
                    Description = source.RoomDescription,
                    State = Enum.GetName(typeof(RoomStatus), source.RoomStatus),
                    CustomerCount = source.GuestCount,
                    BedRoomCount = source.BedroomCount,
                    BedCount = source.BedCount,
                    BathroomCount = source.BathroomCount,
                    District = source.DistrictName,
                    City = source.CityName,
                    Services = source.RoomAmenities.Select(ra => ra.Amentity?.AmentityName ?? "no amentit").ToList(),
                }).ToList(),
                SourceCount = $"{sources.Count}個房源"
            };

            return result;
        }

        public async Task<CalendarViewModel> GetCalendarPrice(int memberId)
        {
            var rooms = await _roomPriceQueryService.GetRoomsByMemberId(memberId);
            //TimeSpan differTime = DateTime.Now - DateTime.UtcNow;
            foreach (var room in rooms)
            {
                var roomPrices = await _roomPriceQueryService.GetIndividualRoomPrice(room.Id);
                //foreach(var roomPrice in room.RoomPrices)
                //{
                //    roomPrice.Date += differTime;
                //}
            }

            List<MemberRoom> memberRoom = rooms.Select(r => new MemberRoom
            {
                RoomId = r.Id,
                FixedPrice = r.FixedPrice,
                WeekendPrice = r.WeekendPrice,
                Name = r.RoomName,
                IndividualPrices = r.RoomPrices.Select(rp => new IndividualPrice
                {
                    Date = rp.Date,
                    SeparatePrice = rp.SeparatePrice
                }).ToList(),
            }).ToList();

            return new CalendarViewModel()
            {
                Rooms = memberRoom
            };
        }
    }


    public class FakeHostingSourceViewModelService : IHostingSourceViewModelService
    {
        public SourceViewModel GetSource(int hostingId)//GetRoomSourceById(int id)
        {
            return new OpenRoom.Web.ViewModels.Hosting.SourceViewModel
            {

                Sources = new List<RoomSource>
                {
                    new RoomSource() {
                        ImgUrl="https://picsum.photos/30/30/?random=66",
                        Category = "露營車",
                        Name = "山明...",
                        Description = "良好",
                        State = "建立中",
                        CustomerCount =3,
                        BedRoomCount =2,
                        BedCount =1,
                        BathroomCount =2,
                        //Services = new List<string>{"Wifi","TV","Kitchen","WashMachine","Parking","AC","SwimmingPool","Bathtub","Garden","BBQ","Dining","Fire","SmokeDetector","FirstAdKit","CoAlarm","FireExtinguisher","FirstAdKit","CoAlarm","FireExtinguisher","CCTV","Weapon","DangerousAnimal"},
                        Services = new List<string>{"Wifi",",TV"},
                        District = "中正區",
                        City = "台灣",
                        //ModifiedTime = DateTime.Now,
                        Id = 1,

                    },
                    new RoomSource() {
                        ImgUrl="https://picsum.photos/30/30/?random=88",
                        Category = "公寓",
                        Name = "清淨...",
                        Description = "好鄰居...",
                        State = "已完成",
                        CustomerCount =2,
                        BedRoomCount =2,
                        BedCount =1,
                        BathroomCount =1,
                        //Services = new List<string>{"Wifi","TV","Kitchen","WashMachine","Parking","AC","SwimmingPool","Bathtub","Garden","BBQ","Dining","Fire","SmokeDetector","FirstAdKit","CoAlarm","FireExtinguisher","FirstAdKit","CoAlarm","FireExtinguisher","CCTV","Weapon","DangerousAnimal"},
                        Services = new List<string>{"Wifi",",TV..."},
                        District = "中山區",
                        City = "台灣",
                        //ModifiedTime = DateTime.Now,
                        Id=2,

                    },
                    new RoomSource() {
                        ImgUrl="https://picsum.photos/30/30/?random=68",
                        Category = "小木屋",
                        Name = "寧靜...",
                        Description = "空氣好...",
                        State = "已完成",
                        CustomerCount =3,
                        BedRoomCount =2,
                        BedCount =1,
                        BathroomCount =1,
                        //Services = new List<string>{"Wifi","TV","Kitchen","WashMachine","Parking","AC","SwimmingPool","Bathtub","Garden","BBQ","Dining","Fire","SmokeDetector","FirstAdKit","CoAlarm","FireExtinguisher","FirstAdKit","CoAlarm","FireExtinguisher","CCTV","Weapon","DangerousAnimal"},
                        Services = new List<string>{"Wifi"},
                        District = "永和區",
                        City = "台灣",
                        //ModifiedTime = DateTime.Now,
                        Id=3,

                    },

                },
                SourceCount = "3個房源",

            };
        }

        public ReservationViewModel GetReservations(int hostingId)
        {

            var viewModel = new ReservationViewModel
            {
                MemberName = "Vivian",
                UpcomingCheckIns = new List<SourceListOrderViewModel>
                {
                    //將入住的訂單資訊
                    new SourceListOrderViewModel
                    {
                        OrderId = 18,
                        RoomName = "靜謐莊園",
                        GuestName = "張小明",
                        CheckInDate = DateTime.Now.AddDays(1), // 假設明天入住
                        CheckOutDate = DateTime.Now.AddDays(4), // 三天後退房
                        Price = 1500m
                    },
                    new SourceListOrderViewModel
                    {
                        OrderId = 20,
                        RoomName = "便利公寓",
                        GuestName = "王小明",
                        CheckInDate = DateTime.Now.AddDays(2), // 假設兩天後入住
                        CheckOutDate = DateTime.Now.AddDays(6), // 五天後退房
                        Price = 2600m
                    }

                },
                CurrentStays = new List<SourceListOrderViewModel>
                {
                    // 目前接待中的訂單資訊
                     new SourceListOrderViewModel
                    {
                        OrderId = 7,
                        RoomName = "山景房",
                        GuestName = "李小明",
                        CheckInDate = DateTime.Now.AddDays(-1),
                        CheckOutDate = DateTime.Now.AddDays(2),
                        Price = 1200m
                    },
                     new SourceListOrderViewModel
                    {
                        OrderId = 8,
                        RoomName = "城市景觀房",
                        GuestName = "楊小明",
                        CheckInDate = DateTime.Now.AddDays(-2),
                        CheckOutDate = DateTime.Now.AddDays(3),
                        Price = 2000m
                    },

                },
                UpcomingCheckOuts = new List<SourceListOrderViewModel>
                {
                    // 即將退房的訂單資訊
                    new SourceListOrderViewModel
                    {
                        OrderId = 1,
                        RoomName = "豪華套房",
                        GuestName = "林小明",
                        CheckInDate = DateTime.Now.AddDays(-3),
                        CheckOutDate = DateTime.Now.AddDays(1),
                        Price = 3000m
                    },
                    new SourceListOrderViewModel
                    {
                        OrderId = 2,
                        RoomName = "田園民宿",
                        GuestName = "孫小明",
                        CheckInDate = DateTime.Now.AddDays(-4),
                        CheckOutDate = DateTime.Now.AddDays(2),
                        Price = 1600m
                    },

                },

            };

            return viewModel;
        }

        //private string GenerateTimeOptions(TimeSpan selectedTime, int startHour = 0, int endHour = 23)
        //{
        //    var optionsHtml = new StringBuilder();
        //    for (int hour = startHour; hour <= endHour; hour++)
        //    {
        //        var isSelected = selectedTime.Hours == hour;
        //        var timeValue = $"{hour:D2}:00"; // Ensuring two-digit hour format
        //        optionsHtml.Append($"<option value='{timeValue}' {(isSelected ? "selected='selected'" : "")}>{timeValue}</option>");
        //    }
        //    return optionsHtml.ToString();
        //}
        //// 處理表單提交的後端方法
        //public void ProcessUserSelection(string selectedTimeOption)
        //{
        //    // 解析前端發送的時間選擇（例如 "14:00"）
        //    TimeSpan selectedTime = TimeSpan.Parse(selectedTimeOption);

        //    // 使用 DateTime.Now 獲取當前日期
        //    DateTime currentDate = DateTime.Now.Date;

        //    // 結合當前日期和用戶選擇的時間
        //    DateTime dateTimeToStore = currentDate.Add(selectedTime);

        //    // 現在你可以將 dateTimeToStore 存入資料庫
        //    // 存入資料庫的代碼依賴於你所使用的數據訪問技術
        //}
        public HostDetailsViewModel GetHostDetails(int roomId)
        {
            var hostDetails = new HostDetailsViewModel
            {
                // Initialize your model with values as needed
                Name = "山明水秀小屋",
                ImageUrls = new List<string>
                {
                     "https://picsum.photos/30/30/?random=88",
                     "https://picsum.photos/30/30/?random=88",
                     "https://picsum.photos/30/30/?random=88",
                     "https://picsum.photos/30/30/?random=88",
                     "https://picsum.photos/30/30/?random=88",
                },
                Title = "舒適山景房",
                Description = "這間獨特的房源地點便利，讓你能輕易安排旅程。",
                GuestNumber = 4,
                BedroomCount = 2,
                BedCount = 3,
                BathroomCount = 1,
                State = 1,
                Category = "7",
                Address = new AddressDto { Country = "台灣", City = "台北", StreetAddress = "中正路", District = "中山區", PostalCode = "123" },
                StreetDescription = "靠近市中心，周邊有多個旅遊景點。",
                Transportation = "公共運輸便利，靠近捷運站。",

                CheckInStartTimeString = "14:00",
                CheckInEndTimeString = "00:00",
                CheckOutTimeString = "12:00",
                Amenities = new List<string> { "Wifi", "電視" },

                //CheckInStartTime = new TimeSpan(14, 0, 0),
                //CheckInEndTime = new TimeSpan(0, 0, 0),
                //CheckOutTime = new TimeSpan(12, 0, 0),
                //    Amenities = new List<Amentity>
                //{
                //    new Amentity { Name = "Wifi"},
                //    new Amentity { Name = "電視"},
                //new Amentity { Name = "廚房", IsAvailable = true },
                //new Amentity { Name = "洗衣機", IsAvailable = false },
                //new Amentity { Name = "室內免費停車", IsAvailable = true },
                //new Amentity { Name = "空調設備", IsAvailable = false },
                //new Amentity { Name = "游泳池", IsAvailable = true },
                //new Amentity { Name = "按摩浴缸", IsAvailable = false },
                //new Amentity { Name = "庭院", IsAvailable = true },
                //new Amentity { Name = "烤肉區", IsAvailable = false },
                //new Amentity { Name = "戶外用餐區", IsAvailable = true },
                //new Amentity { Name = "火坑", IsAvailable = false },
                //new Amentity { Name = "煙霧警報器", IsAvailable = true },
                //new Amentity { Name = "急救包", IsAvailable = false },
                //new Amentity { Name = "一氧化碳警報器", IsAvailable = true },
                //new Amentity { Name = "滅火器", IsAvailable = false },
                //new Amentity { Name = "監視錄影器", IsAvailable = true },
                //new Amentity { Name = "武器", IsAvailable = false },
                //new Amentity { Name = "危險動物", IsAvailable = true },


                //}
            };

            //hostDetails.CheckInStartTimeOptions = GenerateTimeOptions(hostDetails.CheckInStartTime, 8, 23);
            //hostDetails.CheckInEndTimeOptions = GenerateTimeOptions(hostDetails.CheckInEndTime, 0, 23); // Adjusted to allow for 24h format
            //hostDetails.CheckOutTimeOptions = GenerateTimeOptions(hostDetails.CheckOutTime, 0, 23);


            return hostDetails;
        }

        public Task<SourceViewModel> GetSourceAsync(int hostingId)
        {
            throw new NotImplementedException();
        }
        public async Task<CalendarViewModel> GetCalendarPrice(int roomId)
        {
            throw new NotImplementedException();
        }

        public void DeleteRoom(int roomId)
        {
            throw new NotImplementedException();
        }
    }
}
