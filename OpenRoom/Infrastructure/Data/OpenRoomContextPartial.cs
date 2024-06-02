using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public partial class OpenRoomContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AmentityType>().HasData(
                new AmentityType { Id = 1, AmentityName = "設備" },
                new AmentityType { Id = 2, AmentityName = "服務" },
                new AmentityType { Id = 3, AmentityName = "安全" }
            );

            modelBuilder.Entity<LanguageSpeaker>().HasData(
                new LanguageSpeaker { Id = 1, Language = 0, MemberId = 1, },
                new LanguageSpeaker { Id = 2, Language = 1, MemberId = 1, },
                new LanguageSpeaker { Id = 3, Language = 0, MemberId = 2, },
                new LanguageSpeaker { Id = 4, Language = 2, MemberId = 2, },
                new LanguageSpeaker { Id = 5, Language = 0, MemberId = 3, },
                new LanguageSpeaker { Id = 6, Language = 3, MemberId = 3, },
                new LanguageSpeaker { Id = 7, Language = 0, MemberId = 4, },
                new LanguageSpeaker { Id = 8, Language = 4, MemberId = 4, },
                new LanguageSpeaker { Id = 9, Language = 0, MemberId = 5, },
                new LanguageSpeaker { Id = 10, Language = 5, MemberId = 5, }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    Id = 1,
                    FirstName = "Leonardo",
                    LastName = "Pikachu",
                    Email = "leonardopikachu@smail.com",
                    Mobile = "0933456789",
                    PhoneNumber = "0933456789",
                    SelfIntroduction = "是在地的台中人。喜歡到處旅遊親近自然，搜挖各地美食與文化。跟大部份喜愛旅遊的人一樣，從事美術設計與創作的事業。相逢相識即是緣分，歡迎光臨指教。",
                    CreatedAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Password = "12345678",
                    Job = "自由工作者",
                    Live = "台中市, 臺灣",
                    Obsession = "探索世界",
                    Pet = "我的狐狸貓叫做寶貝",
                    Avatar = "https://picsum.photos/240/240/?random=8",
                    AccountStatus = 0,
                    CountryName = "臺灣",
                    CityName = "台中市",
                    Street = "建國路111號",
                    DistrictName = "中區",
                    PostalCode = "40043",
                    Latitude = "24.1367091",
                    Longitude = "120.6807817"
                },
                new Member
                {
                    Id = 2,
                    FirstName = "Taylor",
                    LastName = "Swift",
                    Email = "taylorswift@email.com",
                    Mobile = "0955778899",
                    PhoneNumber = "0955778899",
                    SelfIntroduction = "我是一位熱愛攝影的自由工作者,喜歡到處拍攝大自然的美景。平常也會參加一些攝影比賽,希望能藉此認識更多同好。除了攝影,我也很喜歡烹飪,經常嘗試不同國家的料理。",
                    CreatedAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Password = "password123",
                    Job = "自由攝影師",
                    Live = "台北市, 臺灣",
                    Obsession = "攝影、烹飪",
                    Pet = "我有一隻可愛的貓咪",
                    Avatar = "https://picsum.photos/240/240/?random=2",
                    AccountStatus = 0,
                    CountryName = "臺灣",
                    CityName = "台北市",
                    Street = "忠孝東路六段200號",
                    DistrictName = "大安區",
                    PostalCode = "10651",
                    Latitude = "25.0418651",
                    Longitude = "121.5445294"
                },
                new Member
                {
                    Id = 3,
                    FirstName = "小智",
                    LastName = "林",
                    Email = "davidlee@mail.com",
                    Mobile = "0987654321",
                    PhoneNumber = "0987654321",
                    SelfIntroduction = "我是一位熱愛戶外運動的教師,平常假日我都會安排一些戶外活動,像是健行、爬山或是騎自行車。我也很喜歡分享旅遊的經驗,希望能藉此結交更多志同道合的夥伴。",
                    CreatedAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Password = "qwertyui",
                    Job = "教師",
                    Live = "高雄市, 臺灣",
                    Obsession = "戶外運動、旅遊",
                    Pet = "我沒有寵物",
                    Avatar = "https://picsum.photos/240/240/?random=5",
                    AccountStatus = 0,
                    CountryName = "臺灣",
                    CityName = "高雄市",
                    Street = "鼓山區鹽埕區287號",
                    DistrictName = "鼓山區",
                    PostalCode = "80449",
                    Latitude = "22.6402174",
                    Longitude = "120.2690626"
                },
                new Member
                {
                    Id = 4,
                    FirstName = "春嬌",
                    LastName = "余",
                    Email = "sophiawang@gmail.com",
                    Mobile = "0912345678",
                    PhoneNumber = "0912345678",
                    SelfIntroduction = "我是一位熱愛閱讀的文學工作者,平常除了撰寫作品之外,也會參加一些讀書會或是文學講座。我很喜歡認識不同領域的人,互相交流想法和經驗。",
                    CreatedAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Password = "bookworm",
                    Job = "作家",
                    Live = "台南市, 臺灣",
                    Obsession = "閱讀、寫作",
                    Pet = "我有一隻可愛的貴賓狗",
                    Avatar = "https://picsum.photos/240/240/?random=7",
                    AccountStatus = 0,
                    CountryName = "臺灣",
                    CityName = "台南市",
                    Street = "中西區民生路二段86號",
                    DistrictName = "中西區",
                    PostalCode = "70041",
                    Latitude = "22.9952354",
                    Longitude = "120.2095524"
                },
                new Member
                {
                    Id = 5,
                    FirstName = "志明",
                    LastName = "張",
                    Email = "michaelchen@yahoo.com",
                    Mobile = "0976543210",
                    PhoneNumber = "0976543210",
                    SelfIntroduction = "我是一位熱愛音樂的業餘歌手,平常會參加一些歌唱比賽或是在小酒吧駐場演出。除了音樂之外,我也很喜歡旅遊,希望能透過旅行認識更多不同的文化。",
                    CreatedAt = DateTime.Now,
                    EditAt = DateTime.Now,
                    Password = "singerslife",
                    Job = "會計師",
                    Live = "新竹市, 臺灣",
                    Obsession = "音樂、旅遊",
                    Pet = "我有一隻可愛的哈士奇",
                    Avatar = "https://picsum.photos/240/240/?random=3",
                    AccountStatus = 0,
                    CountryName = "臺灣",
                    CityName = "新竹市",
                    Street = "東區光復路二段235號",
                    DistrictName = "東區",
                    PostalCode = "30076",
                    Latitude = "24.8050914",
                    Longitude = "120.9705871"
                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, RoomId = 1, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(2), CustomerCount = 2, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 2, ReceiptNo = "AE000NA01", OrderNo = "OMG000C01", OrderStatus = 4, TotalPrice = 10000m },
                new Order { Id = 2, RoomId = 2, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(4), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 1, ReceiptNo = "AE000NA02", OrderNo = "OMG000C02", OrderStatus = 4, TotalPrice = 20000m },
                new Order { Id = 3, RoomId = 3, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(5), CustomerCount = 4, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 3, ReceiptNo = "AE000NA03", OrderNo = "OMG000C03", OrderStatus = 4, TotalPrice = 30000m },
                new Order { Id = 4, RoomId = 3, CheckIn = DateTime.Now.AddDays(4), CheckOut = DateTime.Now.AddDays(7), CustomerCount = 4, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 3, ReceiptNo = "AE000NA03", OrderNo = "OMG000C03", OrderStatus = 1, TotalPrice = 30000m },
                new Order { Id = 5, RoomId = 4, CheckIn = DateTime.Now.AddDays(6), CheckOut = DateTime.Now.AddDays(10), CustomerCount = 2, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 5, ReceiptNo = "AE000NA04", OrderNo = "OMG000C04", OrderStatus = 1, TotalPrice = 40000m },
                new Order { Id = 6, RoomId = 5, CheckIn = DateTime.Now.AddDays(8), CheckOut = DateTime.Now.AddDays(12), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 4, ReceiptNo = "AE000NA05", OrderNo = "OMG000C05", OrderStatus = 1, TotalPrice = 50000m },
                new Order { Id = 7, RoomId = 5, CheckIn = DateTime.Now.AddDays(8), CheckOut = DateTime.Now.AddDays(12), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 2, ReceiptNo = "AE000NA05", OrderNo = "OMG000C05", OrderStatus = 1, TotalPrice = 50000m },
                new Order { Id = 8, RoomId = 5, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(12), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 2, ReceiptNo = "AE000NA05", OrderNo = "OMG000C05", OrderStatus = 1, TotalPrice = 50000m },
                new Order { Id = 9, RoomId = 5, CheckIn = DateTime.Now, CheckOut = DateTime.Now.AddDays(12), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 2, ReceiptNo = "AE000NA05", OrderNo = "OMG000C05", OrderStatus = 2, TotalPrice = 50000m },
                new Order { Id = 10, RoomId = 5, CheckIn = DateTime.Now.AddDays(2), CheckOut = DateTime.Now.AddDays(12), CustomerCount = 3, PaymentMethod = 1, CreatedAt = DateTime.Now, MemberId = 2, ReceiptNo = "AE000NA05", OrderNo = "OMG000C05", OrderStatus = 3, TotalPrice = 50000m }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    RoomName = "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）",
                    RoomDescription = "乾淨 整潔 簡約 舒適 溫馨 雙人套房 ,房間乾淨清雅,每個房間皆是溫馨的和室木地板.有32吋液晶電視,小冰箱,冷氣,衛浴設備,距離火車站徒步約8分鐘,住宿地方徒步5分鐘有\"宮原眼科\"冰品 綠川廊道景點,想逛美食\"一中商圈\"徒步約15分鐘  整條皆為可吃可逛的不夜城。<br>*附近有收費停車場，停車方便。附近有多線公車直達-逢甲夜市-東海大學-梧棲魚港-彩虹眷村-高美濕地---南投縣、日月潭-埔里-清境農場-溪頭-衫林溪----鹿港小鎮@多處景點。<br><br>#請留意~入住前需先提供您的身份証或健保卡圖供大樓作登記，如不方便提供者請勿訂房，謝謝。<br>###有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房<br>很重要所以說三次，謝謝🙏",
                    GuestCount = 4,
                    BedroomCount = 2,
                    BedCount = 4,
                    BathroomCount = 2,
                    RoomCategoryId = 1,
                    MemberId = 2,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    EditedAt = DateTime.Now,
                    NearyByTrasportation = "16號小築高樓景觀公寓為短期月租套房",
                    LocationDesription = "如何到達高樓景觀公寓?<br>台中火車站：搭乘300~310任何一號的公車於科博館站下車，車程約10分鐘",
                    CheckInStartTime = DateTime.Now,
                    CheckInEndTime = DateTime.Now,
                    CheckOutTime = DateTime.Now,
                    FixedPrice = 9453m,
                    RoomStatus = 2,
                    CountryName = "臺灣",
                    CityName = "台中市",
                    Street = "建國路111號",
                    DistrictName = "中區",
                    PostalCode = "40043",
                    Latitude = "24.1367091",
                    Longitude = "120.6807817"
                },
                new Room
                {
                    Id = 2,
                    RoomName = "台北悠閒悅居 寧靜舒適的都會渡假體驗",
                    RoomDescription = "寬敞明亮的空間,採光良好,室內設計簡約時尚,提供高品質的住宿體驗。客房配備有舒適的雙人床、32吋液晶電視、小型冰箱和無線網路。浴室乾濕分離,備有淋浴設備和沐浴用品。距離捷運站僅步行5分鐘,周邊有許多美食及購物景點。<br><br>適合情侶、朋友或家庭入住,是您在台北市區短期居住的理想選擇。",
                    GuestCount = 2,
                    BedroomCount = 1,
                    BedCount = 1,
                    BathroomCount = 1,
                    RoomCategoryId = 1,
                    MemberId = 3,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    EditedAt = DateTime.Now,
                    NearyByTrasportation = "捷運忠孝新生站步行5分鐘",
                    LocationDesription = "從捷運忠孝新生站出口步行約5分鐘即可抵達",
                    CheckInStartTime = DateTime.Now,
                    CheckInEndTime = DateTime.Now,
                    CheckOutTime = DateTime.Now,
                    FixedPrice = 2800m,
                    RoomStatus = 1,
                    CountryName = "臺灣",
                    CityName = "台北市",
                    Street = "忠孝東路六段200號",
                    DistrictName = "大安區",
                    PostalCode = "10651",
                    Latitude = "25.0418651",
                    Longitude = "121.5445294"
                },
                new Room
                {
                    Id = 3,
                    RoomName = "高雄海景渡假別墅 優閒時光的私密度假勝地",
                    RoomDescription = "獨棟別墅佔地寬廣,室內裝潢現代典雅,戶外備有私人泳池及庭園。客房採用一流設備,提供極致的舒適體驗。親臨此處,遠離城市喧囂,盡情放鬆身心,感受慵懶的渡假氣氛。<br><br>別墅內有多間獨立空調客房,可供家庭或多人入住。周邊環境清幽雅緻,鄰近海邊及知名景點,是您rendered度假的上佳選擇。",
                    GuestCount = 10,
                    BedroomCount = 5,
                    BedCount = 8,
                    BathroomCount = 4,
                    RoomCategoryId = 3,
                    MemberId = 2,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    EditedAt = DateTime.Now,
                    NearyByTrasportation = "自駕車最方便",
                    LocationDesription = "從高雄市區開車約30分鐘可抵達",
                    CheckInStartTime = DateTime.Now,
                    CheckInEndTime = DateTime.Now,
                    CheckOutTime = DateTime.Now,
                    FixedPrice = 25000m,
                    RoomStatus = 1,
                    CountryName = "臺灣",
                    CityName = "高雄市",
                    Street = "鼓山區鹽埕區287號",
                    DistrictName = "鼓山區",
                    PostalCode = "80449",
                    Latitude = "22.6402174",
                    Longitude = "120.2690626"
                },
                new Room
                {
                    Id = 4,
                    RoomName = "宜蘭villa渡假會館 環抱大自然的世外桃源",
                    RoomDescription = "會館坐落於翠綠山林間,四周環境遼闊寂靜,室內裝潢採用木質元素,充滿質樸自然的渡假氛圍。提供多種房型,可供家庭或多人入住。室內設施一應俱全,客房寬敞舒適,讓您在此盡情放鬆。<br><br>會館內備有高級餐廳及SPA水療中心,戶外有大片庭園及泳池,無論是安排戶外活動或純粹放空靜心,均是理想之選。",
                    GuestCount = 8,
                    BedroomCount = 4,
                    BedCount = 6,
                    BathroomCount = 3,
                    RoomCategoryId = 3,
                    MemberId = 5,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    EditedAt = DateTime.Now,
                    NearyByTrasportation = "會館提供接駁交通工具",
                    LocationDesription = "詳細位置請洽會館服務人員",
                    CheckInStartTime = DateTime.Now,
                    CheckInEndTime = DateTime.Now,
                    CheckOutTime = DateTime.Now,
                    FixedPrice = 18000m,
                    RoomStatus = 1,
                    CountryName = "臺灣",
                    CityName = "宜蘭縣",
                    Street = "員山鄉錦西村16號",
                    DistrictName = "員山鄉",
                    PostalCode = "26942",
                    Latitude = "24.7807806",
                    Longitude = "121.7316414"
                },
                new Room
                {
                    Id = 5,
                    RoomName = "花蓮秘境villa 深山裡的夢幻度假屋",
                    RoomDescription = "隱身於花蓮深山中的villa,室內裝潢別出心裁,結合當代簡約元素與原木質感,呈現獨特的居住體驗。客房寬敞明亮,戶外環境清幽怡人,提供多種房型,適合家庭或多人入住。<br><br>周邊環境毗鄰山林溪流,會館內還設有私人大眾池、SPA水療等設施,讓您盡情放鬆身心,享受大自然的芬多精。",
                    GuestCount = 6,
                    BedroomCount = 3,
                    BedCount = 4,
                    BathroomCount = 2,
                    RoomCategoryId = 3,
                    MemberId = 2,
                    IsDelete = false,
                    CreatedAt = DateTime.Now,
                    EditedAt = DateTime.Now,
                    NearyByTrasportation = "villa會提供接送服務",
                    LocationDesription = "詳細地點請洽villa服務人員",
                    CheckInStartTime = DateTime.Now,
                    CheckInEndTime = DateTime.Now,
                    CheckOutTime = DateTime.Now,
                    FixedPrice = 22000m,
                    RoomStatus = 1,
                    CountryName = "臺灣",
                    CityName = "花蓮縣",
                    Street = "秀林鄉富世村28號",
                    DistrictName = "秀林鄉",
                    PostalCode = "97858",
                    Latitude = "23.9719939",
                    Longitude = "121.5924542"
                }
            );

            modelBuilder.Entity<RoomAmenity>().HasData(
                new RoomAmenity { Id = 1, RoomId = 1, AmentityId = 1, },
                new RoomAmenity { Id = 2, RoomId = 1, AmentityId = 7, },
                new RoomAmenity { Id = 3, RoomId = 1, AmentityId = 13, },
                new RoomAmenity { Id = 4, RoomId = 2, AmentityId = 2, },
                new RoomAmenity { Id = 5, RoomId = 2, AmentityId = 8, },
                new RoomAmenity { Id = 6, RoomId = 2, AmentityId = 14, },
                new RoomAmenity { Id = 7, RoomId = 3, AmentityId = 3, },
                new RoomAmenity { Id = 8, RoomId = 3, AmentityId = 9, },
                new RoomAmenity { Id = 9, RoomId = 3, AmentityId = 15, },
                new RoomAmenity { Id = 10, RoomId = 4, AmentityId = 4, },
                new RoomAmenity { Id = 11, RoomId = 4, AmentityId = 10, },
                new RoomAmenity { Id = 12, RoomId = 4, AmentityId = 16, },
                new RoomAmenity { Id = 13, RoomId = 5, AmentityId = 5, },
                new RoomAmenity { Id = 14, RoomId = 5, AmentityId = 11, },
                new RoomAmenity { Id = 15, RoomId = 5, AmentityId = 17, }
            );

            modelBuilder.Entity<RoomAmentityCategory>().HasData(
                new RoomAmentityCategory { Id = 1, AmentityName = "Wifi", Icon= """<span class="material-symbols-outlined">wifi</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 2, AmentityName = "電視", Icon= """<span class="material-symbols-outlined">tv</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 3, AmentityName = "廚房", Icon = """<span class="material-symbols-outlined">cooking</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 4, AmentityName = "洗衣機", Icon = """<span class="material-symbols-outlined">local_laundry_service</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 5, AmentityName = "室內免費停車", Icon = """<span class="material-symbols-outlined">local_parking</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 6, AmentityName = "空調設備", Icon = """<span class="material-symbols-outlined">ac_unit</span>""", AmentityTypeId = 1 },
                new RoomAmentityCategory { Id = 7, AmentityName = "游泳池", Icon = """<span class="material-symbols-outlined">pool</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 8, AmentityName = "按摩浴缸", Icon = """<span class="material-symbols-outlined">hot_tub</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 9, AmentityName = "庭院", Icon = """<span class="material-symbols-outlined">outdoor_garden</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 10, AmentityName = "烤肉區", Icon = """<span class="material-symbols-outlined">outdoor_grill</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 11, AmentityName = "戶外用餐區", Icon = """<span class="material-symbols-outlined">deck</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 12, AmentityName = "火坑", Icon = """<span class="material-symbols-outlined">local_fire_department</span>""", AmentityTypeId = 2 },
                new RoomAmentityCategory { Id = 13, AmentityName = "煙霧警報器", Icon = """<span class="material-symbols-outlined">detector_smoke</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 14, AmentityName = "急救包", Icon = """<span class="material-symbols-outlined">medical_services</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 15, AmentityName = "一氧化碳警報器", Icon = """<span class="material-symbols-outlined">detector_alarm</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 16, AmentityName = "滅火器", Icon = """<span class="material-symbols-outlined">fire_extinguisher</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 17, AmentityName = "監視錄影器", Icon = """<span class="material-symbols-outlined">motion_sensor_ative</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 18, AmentityName = "武器", Icon = """<span class="material-symbols-outlined">swords</span>""", AmentityTypeId = 3 },
                new RoomAmentityCategory { Id = 19, AmentityName = "危險動物", Icon = """<span class="material-symbols-outlined">pets</span>""", AmentityTypeId = 3 }
            );

            modelBuilder.Entity<RoomCategory>().HasData(
                new RoomCategory { Id = 1, RoomCategory1 = "公寓 Apartment", Icon = """<span class="material-symbols-outlined">apartment</span>""", Sort = 1 },
                new RoomCategory { Id = 2, RoomCategory1 = "獨棟 House", Icon = """<span class="material-symbols-outlined">house</span>""", Sort = 2 },
                new RoomCategory { Id = 3, RoomCategory1 = "家庭式 Home", Icon= """<span class="material-symbols-outlined">night_shelter</span>""", Sort = 3 },
                new RoomCategory { Id = 4, RoomCategory1 = "精品 Luxury", Icon= """<span class="material-symbols-outlined">bedroom_parent</span>""", Sort = 4 },
                new RoomCategory { Id = 5, RoomCategory1 = "莊園 Garden", Icon = """<span class="material-symbols-outlined">home_and_garden</span>""", Sort = 5 },
                new RoomCategory { Id = 6, RoomCategory1 = "民宿 BNB", Icon = """<span class="material-symbols-outlined">villa</span>""", Sort = 6 },
                new RoomCategory { Id = 7, RoomCategory1 = "小木屋 Cabin",Icon= """<span class="material-symbols-outlined">cabin</span>""", Sort = 7 },
                new RoomCategory { Id = 8, RoomCategory1 = "帳篷 Camp", Icon= """<span class="material-symbols-outlined">camping</span>""", Sort = 8 },
                new RoomCategory { Id = 9, RoomCategory1 = "露營車CamperVan", Icon= """<span class="material-symbols-outlined">airport_shuttle</span>""", Sort = 9 }
            );

            modelBuilder.Entity<RoomImage>().HasData(
                new RoomImage { Id = 1, ImageUrl = "https://picsum.photos/600/900/?random=1", RoomId = 1, Sort = 1 },
                new RoomImage { Id = 2, ImageUrl = "https://picsum.photos/900/600/?random=2", RoomId = 1, Sort = 2 },
                new RoomImage { Id = 3, ImageUrl = "https://picsum.photos/900/600/?random=3", RoomId = 1, Sort = 3 },
                new RoomImage { Id = 4, ImageUrl = "https://picsum.photos/900/600/?random=4", RoomId = 1, Sort = 4 },
                new RoomImage { Id = 5, ImageUrl = "https://picsum.photos/900/600/?random=5", RoomId = 1, Sort = 5 },
                new RoomImage { Id = 6, ImageUrl = "https://picsum.photos/600/900/?random=6", RoomId = 1, Sort = 6 },
                new RoomImage { Id = 7, ImageUrl = "https://picsum.photos/900/600/?random=7", RoomId = 1, Sort = 7 },
                new RoomImage { Id = 8, ImageUrl = "https://picsum.photos/600/900/?random=8", RoomId = 2, Sort = 1 },
                new RoomImage { Id = 9, ImageUrl = "https://picsum.photos/900/600/?random=9", RoomId = 2, Sort = 2 },
                new RoomImage { Id = 10, ImageUrl = "https://picsum.photos/900/600/?random=10", RoomId = 2, Sort = 3 },
                new RoomImage { Id = 11, ImageUrl = "https://picsum.photos/900/600/?random=11", RoomId = 2, Sort = 4 },
                new RoomImage { Id = 12, ImageUrl = "https://picsum.photos/900/600/?random=12", RoomId = 2, Sort = 5 },
                new RoomImage { Id = 13, ImageUrl = "https://picsum.photos/600/900/?random=13", RoomId = 2, Sort = 6 },
                new RoomImage { Id = 14, ImageUrl = "https://picsum.photos/900/600/?random=14", RoomId = 2, Sort = 7 },
                new RoomImage { Id = 15, ImageUrl = "https://picsum.photos/600/900/?random=15", RoomId = 3, Sort = 1 },
                new RoomImage { Id = 16, ImageUrl = "https://picsum.photos/900/600/?random=16", RoomId = 3, Sort = 2 },
                new RoomImage { Id = 17, ImageUrl = "https://picsum.photos/900/600/?random=17", RoomId = 3, Sort = 3 },
                new RoomImage { Id = 18, ImageUrl = "https://picsum.photos/900/600/?random=18", RoomId = 3, Sort = 4 },
                new RoomImage { Id = 19, ImageUrl = "https://picsum.photos/900/600/?random=19", RoomId = 3, Sort = 5 },
                new RoomImage { Id = 20, ImageUrl = "https://picsum.photos/600/900/?random=20", RoomId = 3, Sort = 6 },
                new RoomImage { Id = 21, ImageUrl = "https://picsum.photos/900/600/?random=21", RoomId = 3, Sort = 7 },
                new RoomImage { Id = 22, ImageUrl = "https://picsum.photos/600/900/?random=22", RoomId = 4, Sort = 1 },
                new RoomImage { Id = 23, ImageUrl = "https://picsum.photos/900/600/?random=23", RoomId = 4, Sort = 2 },
                new RoomImage { Id = 24, ImageUrl = "https://picsum.photos/900/600/?random=24", RoomId = 4, Sort = 3 },
                new RoomImage { Id = 25, ImageUrl = "https://picsum.photos/900/600/?random=25", RoomId = 4, Sort = 4 },
                new RoomImage { Id = 26, ImageUrl = "https://picsum.photos/900/600/?random=26", RoomId = 4, Sort = 5 },
                new RoomImage { Id = 27, ImageUrl = "https://picsum.photos/600/900/?random=27", RoomId = 4, Sort = 6 },
                new RoomImage { Id = 28, ImageUrl = "https://picsum.photos/900/600/?random=28", RoomId = 4, Sort = 7 },
                new RoomImage { Id = 29, ImageUrl = "https://picsum.photos/600/900/?random=29", RoomId = 5, Sort = 1 },
                new RoomImage { Id = 30, ImageUrl = "https://picsum.photos/900/600/?random=30", RoomId = 5, Sort = 2 },
                new RoomImage { Id = 31, ImageUrl = "https://picsum.photos/900/600/?random=31", RoomId = 5, Sort = 3 },
                new RoomImage { Id = 32, ImageUrl = "https://picsum.photos/900/600/?random=32", RoomId = 5, Sort = 4 },
                new RoomImage { Id = 33, ImageUrl = "https://picsum.photos/900/600/?random=33", RoomId = 5, Sort = 5 },
                new RoomImage { Id = 34, ImageUrl = "https://picsum.photos/600/900/?random=34", RoomId = 5, Sort = 6 },
                new RoomImage { Id = 35, ImageUrl = "https://picsum.photos/900/600/?random=35", RoomId = 5, Sort = 7 },
                new RoomImage { Id = 36, ImageUrl = "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943619/uploadFolder/pixlr-image-generator-65c4bc723adf8c016f2dcfe7_2e9fb275-6317-49f5-8888-82e68c5964d4.png", RoomId = 5, Sort = 8 },
                new RoomImage { Id = 37, ImageUrl = "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943620/uploadFolder/sample_logo_e328c061-c25a-42d1-817a-10a0ec1e723c.png", RoomId = 5, Sort = 9 },
                new RoomImage { Id = 38, ImageUrl = "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943621/uploadFolder/step1_dc783258-4498-44ec-9f66-ac2256467667.webp", RoomId = 5, Sort = 10 },
                new RoomImage { Id = 39, ImageUrl = "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943621/uploadFolder/step2_b6792034-4d9f-48a9-aa69-944e7222d785.webp", RoomId = 5, Sort = 11 },
                new RoomImage { Id = 40, ImageUrl = "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943622/uploadFolder/step3_cb727d36-409a-41c5-a8cb-91bb99075777.webp", RoomId = 5, Sort = 12 }


            );

            modelBuilder.Entity<RoomReview>().HasData(
                new RoomReview { Id = 1, RatingScore = 5, ReviewContent = "整潔度整體還不錯", CreatedAt = DateTime.Now },
                new RoomReview { Id = 2, RatingScore = 5, ReviewContent = "整潔度整體還不錯", CreatedAt = DateTime.Now },
                new RoomReview { Id = 3, RatingScore = 5, ReviewContent = "整潔度整體還不錯", CreatedAt = DateTime.Now },
                new RoomReview { Id = 4, RatingScore = 5, ReviewContent = "整潔度整體還不錯", CreatedAt = DateTime.Now },
                new RoomReview { Id = 5, RatingScore = 5, ReviewContent = "整潔度整體還不錯", CreatedAt = DateTime.Now }
            );

            modelBuilder.Entity<Wishlist>().HasData(
                new Wishlist { Id = 1, MemberId = 1, WishlistName = "彭于晏住我隔壁房" },
                new Wishlist { Id = 2, MemberId = 2, WishlistName = "今晚不想回家系列" },
                new Wishlist { Id = 3, MemberId = 1, WishlistName = "說走就走~ 我有的是時間" },
                new Wishlist { Id = 4, MemberId = 3, WishlistName = "給我海闊天空的景色" },
                new Wishlist { Id = 5, MemberId = 2, WishlistName = "初戀粉色系" }
            );

            modelBuilder.Entity<WishlistDetail>().HasData(
                new WishlistDetail { Id = 1, WishlistId = 5, RoomId = 3 },
                new WishlistDetail { Id = 2, WishlistId = 5, RoomId = 2 },
                new WishlistDetail { Id = 3, WishlistId = 3, RoomId = 1 },
                new WishlistDetail { Id = 4, WishlistId = 3, RoomId = 2 },
                new WishlistDetail { Id = 5, WishlistId = 1, RoomId = 4 }
            );
        }
    }
}
