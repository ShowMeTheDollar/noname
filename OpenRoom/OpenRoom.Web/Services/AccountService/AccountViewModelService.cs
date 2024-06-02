
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ApplicationCore.Enums;
using OpenRoom.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Drawing;

#nullable disable
namespace OpenRoom.Web.Services.AccountService
{
    public class AccountViewModelService : IAccountViewModelService
    {
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<LanguageSpeaker> _languageRepo;
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<RoomImage> _roomImageRepo;
        private readonly IRepository<RoomCategory> _roomCategoryRepo;
        public AccountViewModelService(IRepository<Member> memberRepo, IRepository<LanguageSpeaker> languageRepo, IRepository<Room> roomRepo, IRepository<RoomImage> roomImageRepo, IRepository<RoomCategory> roomCategoryRepo)
        {
            _memberRepo = memberRepo;
            _languageRepo = languageRepo;
            _roomRepo = roomRepo;
            _roomImageRepo = roomImageRepo;
            _roomCategoryRepo = roomCategoryRepo;
        }

        public AccountDataViewModel GetAccountCard()
        {
            return new AccountDataViewModel()
            {
                AccountCards = GetAccountCardList(),
            };
        }

        public async Task<PersonalViewModel> GetPersonalData(int? userId)
        {
            var member = await _memberRepo.GetByIdAsync(userId);
            if (member == null)
            {
                return null;
            }

            return new PersonalViewModel()
            {
                Id = member.Id,
                OldPassword = member.Password,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                CountryName = member.CountryName,
                CityName = member.CityName,
                DistrictName = member.DistrictName,
                Street = member.Street,
                PostalCode = member.PostalCode,
                EmergencyContact = member.EmergencyContact,
                EmergencyNumber = member.EmergencyNumber
            };
        }

        public async Task<UserProfileViewModel> GetUserProfileViewModel(int userId)
        {
            var member = await _memberRepo.GetByIdAsync(userId);
            if (member == null)
            {
                return null;
            }
            var languages = _languageRepo.List(m => m.MemberId == userId);

            return new UserProfileViewModel()
            {
                LandlordName = $"{member.FirstName} {member.LastName}",
                AvatarUrl = member.Avatar,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                Job = member.Job,
                Live = member.Live,
                Obsession = member.Obsession,
                Pet = member.Pet,
                Language = string.Join(" and ", languages.Select(l => (Languages)l.Language)),
                SelfIntroduction = member.SelfIntroduction,
                ReviewCards = GetReviewCardsList(),
                RoomCards = GetRoomCardsList(member.Id),
            };
        }

        private List<AccountCardViewModel> GetAccountCardList()
        {
            return new List<AccountCardViewModel>
            {
                new AccountCardViewModel
                {
                    InnerLink = "/AccountSettings/Personal",
                    WebIconClass = "fa-solid fa-id-card fa-2x",
                    PhoneIconClass ="fa-solid fa-user",
                    AccountTitle = "個人資料",
                    AccountContent = "提供個人資料和連絡方式"
                },
                new AccountCardViewModel
                {
                    InnerLink = "/AccountSettings/LoginAndSecurity",
                    WebIconClass = "fa-solid fa-shield-halved fa-2x",
                    PhoneIconClass ="fa-solid fa-gear",
                    AccountTitle = "登入與安全",
                    AccountContent = "更新密碼以確保帳號安全"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-money-bills fa-2x",
                    AccountTitle = "付款與收款",
                    AccountContent = "查看付款、收款、優惠券和禮物卡"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-regular fa-file fa-2x",
                    AccountTitle = "稅費",
                    AccountContent = "管理納稅人資料和稅務文件"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-bullhorn fa-2x",
                    AccountTitle = "通知",
                    AccountContent = "選擇通知設定和你偏好的聯絡方式"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-regular fa-eye fa-2x",
                    AccountTitle = "隱私與分享",
                    AccountContent = "管理你的個人資料、已連結的服務與資料分享設定"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-toggle-on fa-2x",
                    AccountTitle = "全域偏好設定",
                    AccountContent = "設定預設的語言、幣別和時區"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-business-time fa-2x",
                    AccountTitle = "差旅",
                    AccountContent = "新增工作電子郵件，即可享受商務差旅福利"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-chart-simple fa-2x",
                    AccountTitle = "專業出租工具",
                    AccountContent = "如果你在 Openroom 上管理多間房源，可以使用專業工具"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-hands-holding-circle fa-2x",
                    AccountTitle = "邀請好友旅行基金和優惠券",
                    AccountContent = "你有 $0 TWD 的邀請好友旅行基金和優惠券可供使用。了解詳情。"
                }
            };
        }

        private List<RoomCardViewModel> GetRoomCardsList(int memberId)
        {
            var rooms = _roomRepo.List(r => r.MemberId == memberId);
            var roomImage = _roomImageRepo.List(ri => rooms.Select(r => r.Id).Contains(ri.RoomId));
            var roomCategory = _roomCategoryRepo.List(rc => rooms.Select(r => r.RoomCategoryId).Contains(rc.Id));
            var roomCardViewModels = rooms.Select(r => new RoomCardViewModel
            {
                RoomId = r.Id,
                Title = r.RoomName,
                ImgUrl = r.RoomImages.FirstOrDefault()?.ImageUrl ?? string.Empty,
                CategoryName = r.RoomCategory.RoomCategory1,
                Rating = r.Review
            }).ToList();

            return roomCardViewModels;
        }

        private List<ReviewCardViewModel> GetReviewCardsList()
        {
            return new List<ReviewCardViewModel>
            {
                new ReviewCardViewModel
                {
                    Id = 1,
                    Name = "Leo Pikachu",
                    City = "台北",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=10",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東非常親切，對租客的需求十分關心，總是能提供最佳的租賃解決方案。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 2,
                    Name = "Taylor Swift",
                    City = "新北",
                    rate = 4,
                    ImgUrl = "https://picsum.photos/50/50/?random=11",
                    CreateAt = DateTime.Now, EditAt = DateTime.Now,
                    Description = "房東的服務極其專業，從租賃到維修都能迅速處理，讓我們的生活更加舒適。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 3,
                    Name = "小智",
                    City = "高雄",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=12",
                    CreateAt = DateTime.Now, EditAt = DateTime.Now,
                    Description = "房東的溝通能力很強，即使是在緊急情況下也能迅速解決問題，讓我們感到非常放心。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 4,
                    Name = "春嬌",
                    City = "台中",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=13",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東非常關心租客的生活品質，經常提供各種福利和折扣，讓我們在住宿的同時也能享受到生活的樂趣。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 5,
                    Name = "志明",
                    City = "新竹",
                    rate = 4,
                    ImgUrl = "https://picsum.photos/50/50/?random=14",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東的專業知識和經驗非常豐富，對於房屋的維護和管理都非常到位，讓我們的居住環境極為安全和舒適。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 6,
                    Name = "世堅",
                    City = "桃園",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=15",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東提供的房間環境舒適，設施完備，服務專業。房東關心租客需求，提供優質服務，讓居住生活更加舒適。",
                    IsDelete = false
                }
            };
        }
    }
    public class FakeAccountViewModelService : IAccountViewModelService
    {
        public AccountDataViewModel GetAccountCard()
        {
            return new AccountDataViewModel()
            {
                AccountCards = GetAccountCardList(),

            };
        }

        public async Task<PersonalViewModel> GetPersonalData(int? userId)
        {
            return new PersonalViewModel()
            {
                FirstName = "Test",
                LastName = "Name",
                Email = "test@gmail.com",
                CountryName = "Test Country",
                CityName = "Test City",
                DistrictName = "Test District",
                Street = "Test Street",
                PostalCode = "12345",
                EmergencyContact = "999"
            };
        }

        public async Task<UserProfileViewModel> GetUserProfileViewModel(int userId)
        {
            return new UserProfileViewModel()
            {
                LandlordName = "Leonardo Pikachu",
                AvatarUrl = "https://picsum.photos/240/240/?random=8",
                Job = "自由工作者",
                Language = "中文 and 英語",
                Live = "台中市, 臺灣",
                SelfIntroduction = "房源就在台中火車站斜對面，樓下就是干城車站。是在地的台中人。喜歡到處旅遊親近自然，搜挖各地美食與文化。跟大部份喜愛旅遊的人一樣，從事美術設計與創作的事業。相逢相識即是緣分，歡迎光臨指教。",
                ReviewCards = GetReviewCardsList(),
                RoomCards = GetRoomCardsList(),
            };
        }

        private List<AccountCardViewModel> GetAccountCardList()
        {
            return new List<AccountCardViewModel>
            {
                new AccountCardViewModel
                {
                    InnerLink = "/AccountSettings/Personal",
                    WebIconClass = "fa-solid fa-id-card fa-2x",
                    PhoneIconClass ="fa-solid fa-user",
                    AccountTitle = "個人資料",
                    AccountContent = "提供個人資料和連絡方式"
                },
                new AccountCardViewModel
                {
                    InnerLink = "/AccountSettings/LoginAndSecurity",
                    WebIconClass = "fa-solid fa-shield-halved fa-2x",
                    PhoneIconClass ="fa-solid fa-gear",
                    AccountTitle = "登入與安全",
                    AccountContent = "更新密碼以確保帳號安全"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-money-bills fa-2x",
                    AccountTitle = "付款與收款",
                    AccountContent = "查看付款、收款、優惠券和禮物卡"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-regular fa-file fa-2x",
                    AccountTitle = "稅費",
                    AccountContent = "管理納稅人資料和稅務文件"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-bullhorn fa-2x",
                    AccountTitle = "通知",
                    AccountContent = "選擇通知設定和你偏好的聯絡方式"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-regular fa-eye fa-2x",
                    AccountTitle = "隱私與分享",
                    AccountContent = "管理你的個人資料、已連結的服務與資料分享設定"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-toggle-on fa-2x",
                    AccountTitle = "全域偏好設定",
                    AccountContent = "設定預設的語言、幣別和時區"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-business-time fa-2x",
                    AccountTitle = "差旅",
                    AccountContent = "新增工作電子郵件，即可享受商務差旅福利"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-chart-simple fa-2x",
                    AccountTitle = "專業出租工具",
                    AccountContent = "如果你在 Openroom 上管理多間房源，可以使用專業工具"
                },
                new AccountCardViewModel
                {
                    InnerLink = "#",
                    OnlyShowWeb ="web-card-hyperlink",
                    WebIconClass = "fa-solid fa-hands-holding-circle fa-2x",
                    AccountTitle = "邀請好友旅行基金和優惠券",
                    AccountContent = "你有 $0 TWD 的邀請好友旅行基金和優惠券可供使用。了解詳情。"
                }
            };
        }

        private List<ReviewCardViewModel> GetReviewCardsList()
        {
            return new List<ReviewCardViewModel>
            {
                new ReviewCardViewModel
                {
                    Id = 1, Name = "Leo Pikachu",
                    City = "台北",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=10",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now, Description = "房東非常親切，對租客的需求十分關心，總是能提供最佳的租賃解決方案。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 2,
                    Name = "Taylor Swift",
                    City = "新北",
                    rate = 4,
                    ImgUrl = "https://picsum.photos/50/50/?random=11",
                    CreateAt = DateTime.Now, EditAt = DateTime.Now,
                    Description = "房東的服務極其專業，從租賃到維修都能迅速處理，讓我們的生活更加舒適。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 3,
                    Name = "小智",
                    City = "高雄",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=12",
                    CreateAt = DateTime.Now, EditAt = DateTime.Now,
                    Description = "房東的溝通能力很強，即使是在緊急情況下也能迅速解決問題，讓我們感到非常放心。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 4,
                    Name = "春嬌",
                    City = "台中",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=13",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東非常關心租客的生活品質，經常提供各種福利和折扣，讓我們在住宿的同時也能享受到生活的樂趣。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 5,
                    Name = "志明",
                    City = "新竹",
                    rate = 4,
                    ImgUrl = "https://picsum.photos/50/50/?random=14",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東的專業知識和經驗非常豐富，對於房屋的維護和管理都非常到位，讓我們的居住環境極為安全和舒適。",
                    IsDelete = false
                },
                new ReviewCardViewModel
                {
                    Id = 6,
                    Name = "世堅",
                    City = "桃園",
                    rate = 5,
                    ImgUrl = "https://picsum.photos/50/50/?random=15",
                    CreateAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Description = "房東提供的房間環境舒適，設施完備，服務專業。房東關心租客需求，提供優質服務，讓居住生活更加舒適。",
                    IsDelete = false
                }
            };
        }

        private List<RoomCardViewModel> GetRoomCardsList()
        {
            return new List<RoomCardViewModel>
            {
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=16",
                    CategoryName = "小木屋",
                    Rating = 9.3f
                },
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=17",
                    CategoryName = "帳篷",
                    Rating = 9.8f
                },
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=18",
                    CategoryName = "公寓",
                    Rating = 8.6f
                },
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=19",
                    CategoryName = "家庭式",
                    Rating = 7.6f
                },
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=20",
                    CategoryName = "精品",
                    Rating = 9.0f
                },
                new RoomCardViewModel
                {
                    Title = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    ImgUrl = "https://picsum.photos/250/250/?random=21",
                    CategoryName = "莊園",
                    Rating = 8.4f
                }
            };
        }
    }
}
