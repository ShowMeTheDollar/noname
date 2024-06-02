using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OpenRoom.Web.ViewModels;
using System.IO;

namespace OpenRoom.Web.Services
{
    public class FakeRoomsViewModelService : IRoomsViewModelService
    {
       
        public async Task<RoomDetailViewModel> GetRoomDetailsViewModel(int roomId)
        {
            return new RoomDetailViewModel
            {
                RoomId = 1,
                RoomName = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                RoomImgUrls = new List<string>
                {
                    "https://picsum.photos/900/600/?random=1",
                    "https://picsum.photos/900/600/?random=2",
                    "https://picsum.photos/900/600/?random=3",
                    "https://picsum.photos/900/600/?random=4",
                    "https://picsum.photos/900/600/?random=5"
                },
                CountryName = "臺灣",
                CityName = "台中市",
                DistrictName = "Central District",
                BedroomCount = 2,
                BedCount = 1,
                BathroomCount = 1,
                GuestCount = 2,
                LandlordName = "Emily",
                AvatarUrl = "https://picsum.photos/240/240/?random=8",
                SelfIntroduction = "房源就在台中火車站斜對面，樓下就是干城車站。是在地的台中人。喜歡到處旅遊親近自然，搜挖各地美食與文化。跟大部份喜愛旅遊的人一樣，從事美術設計與創作的事業。相逢相識即是緣分，歡迎光臨指教。",
                Job = "自由工作者",
                Live = "台中市, 臺灣",
                RoomDescription = "乾淨 整潔 簡約 舒適 溫馨 雙人套房 ,房間乾淨清雅,每個房間皆是溫馨的和室木地板.有32吋液晶電視,小冰箱,冷氣,衛浴設備,距離火車站徒步約8分鐘,住宿地方徒步5分鐘有\"宮原眼科\"冰品 綠川廊道景點,想逛美食\"一中商圈\"徒步約15分鐘  整條皆為可吃可逛的不夜城。<br>*附近有收費停車場，停車方便。附近有多線公車直達-逢甲夜市-東海大學-梧棲魚港-彩虹眷村-高美濕地---南投縣、日月潭-埔里-清境農場-溪頭-衫林溪----鹿港小鎮@多處景點。<br><br>#請留意~入住前需先提供您的身份証或健保卡圖供大樓作登記，如不方便提供者請勿訂房，謝謝。<br>###有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房<br>很重要所以說三次，謝謝🙏",
                AmentityName = new List<string>()
                {
                    "吹風機", "洗髮露", "熱水", "生活必需品", "衣架", "電視", "空調設備", "臥室門可上鎖", "房源內的監視錄影器", "冰箱", "電梯", "建築物外有收費停車位", "可存放行李", "自助入住", "大門密碼鎖", "廚房", "WiFi", "洗衣機", "烘衣機", "煙霧警報器", "一氧化碳警報器", "暖氣"
                },
                CheckInTime = DateTime.Today.AddHours(2),
                CheckOutTime = DateTime.Today.AddHours(11),
                FixedPrice = 9453m,
                ReviewCards = GetReviewCardList(),
                NearyByTrasportation = "16號小築高樓景觀公寓為短期月租套房，因位於台中最繁華的市中心,，所以極吸引各地（國）商務客來短期租賃，這裡不僅有金典綠園道、SOGO百貨公司及各種不同風味的餐館,更有令人陶醉的人文氣息,步行5分鐘走進綠園道便可沿途拜訪勤美術館、范特喜文創園區、審計新村文創園區及國立台灣美術館。<br><br>每到週末時分,綠園道便會出現各種特色工藝品的攤販以及讓人目不轉睛的街頭藝人表演,美的文化不斷滋養著這塊寶地,而藝術與創意也正同時地豐富了我們的生活",
                LocationDesription = "如何到達高樓景觀公寓?<br>台中火車站：搭乘300~310任何一號的公車於科博館站下車，車程約10分鐘<br>台中高鐵站：搭乘159號公車於廣三SOGO站下車，車程約20分鐘<br>台中機場：搭乘302號公車於科博館站下車，車程約40分鐘<br>台北轉運站：搭乘國光客運或統聯客運於科博站下車，車程約2小時10分鐘<br><br>周邊景點：<br>步行:5分鐘內可到台中科學博物館、綠園道、勤美誠品、SOGO百貨<br>公車:10分鐘直達新光三越、15分鐘直達逢甲夜市、15分鐘直達一中街商圈、20分鐘直達東海大學、30分鐘直達彩虹眷村、50分鐘直達高美溼地<br><br>旅店門口即是公車站(科博館站)及U-Bike停放站,使用悠遊卡搭乘公車可享每次前10公里免費,使用悠遊卡騎乘U-Bike只要先註冊好便可享每次前30分鐘免費。旅店門口亦有排班的計程車(的士)可搭乘。",
            };
        }

        private List<ReviewCardViewModel> GetReviewCardList()
        {
            return new List<ReviewCardViewModel>()
            {
                new ReviewCardViewModel { Id = 1, Name = "志明",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=10", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "整潔度整體還不錯", IsDelete = false },
                new ReviewCardViewModel { Id = 2, Name = "春嬌",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=11", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "“…尷尬幫他的客人訂房，客人房間弄得髒亂，違反規定在房間內抽煙，不僅延遲非常久退房沒有事先告知還帶走房內用品，雖然後來有歸還，但整個是不好的體驗！…", IsDelete = false },
                new ReviewCardViewModel { Id = 3, Name = "小智",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=12", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "整潔度整體還不錯", IsDelete = false },
                new ReviewCardViewModel { Id = 4, Name = "Taylor Swift",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=13", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "“…尷尬幫他的客人訂房，客人房間弄得髒亂，違反規定在房間內抽煙，不僅延遲非常久退房沒有事先告知還帶走房內用品，雖然後來有歸還，但整個是不好的體驗！…", IsDelete = false },
                new ReviewCardViewModel { Id = 5, Name = "世堅",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=14", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "整潔度整體還不錯", IsDelete = false },
                new ReviewCardViewModel { Id = 6, Name = "Leonardo Pikachu",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=15", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "“…尷尬幫他的客人訂房，客人房間弄得髒亂，違反規定在房間內抽煙，不僅延遲非常久退房沒有事先告知還帶走房內用品，雖然後來有歸還，但整個是不好的體驗！…", IsDelete = false }
            };
        }
    }

    public class RoomsViewModelService : IRoomsViewModelService
    {
        private readonly IRoomQueryService _roomQueryService;
        private readonly IWishCardQueryService _wishCardQueryService;
        private readonly IWishViewModelService _wishViewModelService;
        private readonly int _memberId;
        public RoomsViewModelService(IRoomQueryService roomQueryService, IWishCardQueryService wishCardQueryService, UserService userService, IWishViewModelService wishViewModelService)
        {
            _roomQueryService = roomQueryService;
            _wishCardQueryService = wishCardQueryService;
            _memberId = userService.GetMemberId().GetValueOrDefault();
            _wishViewModelService = wishViewModelService;
            //_memberId = 2;
        }

        public async Task<RoomDetailViewModel> GetRoomDetailsViewModel(int roomId)
        {
            var room = await (_roomQueryService.GetRoomDetails(roomId));
            if (room == null)
            {
                return null;
            }

            var roomDetailViewModel = new RoomDetailViewModel()
            {
                RoomId = room.Id,
                RoomCategoryName = room.RoomCategory.RoomCategory1,
                CustomerId = _memberId,
                RoomName = room.RoomName,
                RoomImgUrls = room.RoomImages.Select(image => image.ImageUrl).ToList(),
                PostalCode = room.PostalCode,
                CountryName = room.CountryName,
                CityName = room.CityName,
                DistrictName = room.DistrictName,
                Street = room.Street,
                BedroomCount = room.BedCount,
                BedCount = room.BedCount,
                BathroomCount = room.BathroomCount,
                GuestCount = room.GuestCount,
                LandlordName = $"{room.Member.FirstName} {room.Member.LastName}",
                LandlordId = room.MemberId,
                AvatarUrl = room.Member.Avatar,
                SelfIntroduction = room.Member.SelfIntroduction,
                Job = room.Member.Job,
                Language = string.Join(" and ", room.Member.LanguageSpeakers.Select(l => (Languages)l.Language)),
                Live = room.Member.Live,
                Obsession = room.Member.Obsession,
                Pet = room.Member.Pet,
                RoomDescription = room.RoomDescription,
                AmentityName = room.RoomAmenities.Select(ra => ra.Amentity.AmentityName).ToList(),
                AmentityIcon = room.RoomAmenities.Select(ra => ra.Amentity.Icon).ToList(),
                AmentityTypeId = room.RoomAmenities.Select(ra => ra.Amentity.AmentityTypeId).ToList(),
                CheckInTime = room.CheckInStartTime,
                CheckOutTime = room.CheckOutTime,
                FixedPrice = room.FixedPrice,
                ReviewCards = GetReviewCardList(),
                NearyByTrasportation = room.NearyByTrasportation,
                LocationDesription = room.LocationDesription,
                Latitude = room.Latitude,
                Longitude = room.Longitude,
                Rating = room?.Review ?? 4.84,
                CheckIn = room.Orders.Select(o => o.CheckIn.AddHours(8)).ToList(),
                CheckOut = room.Orders.Select(o => o.CheckOut.AddHours(8).AddDays(-1)).ToList(),
                CustomCategoryRooms = _wishViewModelService.GetCustomerCategoryRooms()
            };

            return roomDetailViewModel;
        }

       

        private List<ReviewCardViewModel> GetReviewCardList()
        {
            return new List<ReviewCardViewModel>()
            {
                new ReviewCardViewModel { Id = 1, Name = "Leo Pikachu",  City = "台北", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=10", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東非常親切，對租客的需求十分關心，總是能提供最佳的租賃解決方案。", IsDelete = false },
                new ReviewCardViewModel { Id = 2, Name = "Taylor Swift",  City = "新北", rate = 4, ImgUrl = "https://picsum.photos/50/50/?random=11", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東的服務極其專業，從租賃到維修都能迅速處理，讓我們的生活更加舒適。", IsDelete = false },
                new ReviewCardViewModel { Id = 3, Name = "小智",  City = "高雄", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=12", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東的溝通能力很強，即使是在緊急情況下也能迅速解決問題，讓我們感到非常放心。", IsDelete = false },
                new ReviewCardViewModel { Id = 4, Name = "春嬌",  City = "台中", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=13", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東非常關心租客的生活品質，經常提供各種福利和折扣，讓我們在住宿的同時也能享受到生活的樂趣。", IsDelete = false },
                new ReviewCardViewModel { Id = 5, Name = "志明",  City = "新竹", rate = 4, ImgUrl = "https://picsum.photos/50/50/?random=14", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東的專業知識和經驗非常豐富，對於房屋的維護和管理都非常到位，讓我們的居住環境極為安全和舒適。", IsDelete = false },
                new ReviewCardViewModel { Id = 6, Name = "世堅",  City = "桃園", rate = 5, ImgUrl = "https://picsum.photos/50/50/?random=15", CreateAt = DateTime.Now, EditAt = DateTime.Now, Description = "房東提供的房間環境舒適，設施完備，服務專業。房東關心租客需求，提供優質服務，讓居住生活更加舒適。", IsDelete = false }
            };
        }
    }
}
