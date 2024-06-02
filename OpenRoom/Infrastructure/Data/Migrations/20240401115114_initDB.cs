using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AmentityTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmentityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpecificInfo = table.Column<bool>(type: "bit", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmentityTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SelfIntroduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    EditAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Job = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Live = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Obsession = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountStatus = table.Column<int>(type: "int", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoomCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSourceCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThirdPartyLogin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProviderUserID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdPartyLogin", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoomAmentityCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmentityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmentityTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAmentityCategoryies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomAmentityCategoryies_AmentityTypes",
                        column: x => x.AmentityTypeID,
                        principalTable: "AmentityTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LanguageSpeaker",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSpeaker", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LanguageSpeaker_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    WishlistName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Wishlist_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoomDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestCount = table.Column<int>(type: "int", nullable: false),
                    BedroomCount = table.Column<int>(type: "int", nullable: false),
                    BedCount = table.Column<int>(type: "int", nullable: false),
                    BathroomCount = table.Column<int>(type: "int", nullable: false),
                    RoomCategoryID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    EditedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    NearyByTrasportation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationDesription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckInStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckInEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    FixedPrice = table.Column<decimal>(type: "money", nullable: false),
                    WeekendPrice = table.Column<decimal>(type: "money", nullable: true),
                    RoomStatus = table.Column<int>(type: "int", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: true, comment: "自定義排序"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "備註"),
                    Review = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSource", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rooms_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Rooms_RoomCategories",
                        column: x => x.RoomCategoryID,
                        principalTable: "RoomCategories",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MemberThirdPartyLink",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ThirdPartyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberThirdPartyLink", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MemberThirdPartyLink_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MemberThirdPartyLink_ThirdPartyLogin",
                        column: x => x.ThirdPartyID,
                        principalTable: "ThirdPartyLogin",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime", nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, comment: "只有變更才會有值"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ReceiptNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Members",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Orders_Rooms",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RoomAmenities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    AmentityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAmenities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomAmenities_RoomAmentityCategoryies",
                        column: x => x.AmentityID,
                        principalTable: "RoomAmentityCategories",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RoomAmenities_Rooms",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RoomImages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomImages_Rooms",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RoomPrices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    SeparatePrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPrices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomPrices_Rooms",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WishlistDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WishlistID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WishlistDetails_Rooms",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WishlistDetails_Wishlist",
                        column: x => x.WishlistID,
                        principalTable: "Wishlist",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ecpay",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MerchantTradeNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RtnCode = table.Column<int>(type: "int", nullable: true),
                    RtnMsg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TradeNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TradeAmt = table.Column<int>(type: "int", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentTypeChargeFee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TradeDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SimulatePaid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Green", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Green_Orders",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RoomReviews",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    RatingScore = table.Column<int>(type: "int", nullable: false),
                    ReviewContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReviews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomReviews_Orders",
                        column: x => x.ID,
                        principalTable: "Orders",
                        principalColumn: "ID");
                });

            migrationBuilder.InsertData(
                table: "AmentityTypes",
                columns: new[] { "ID", "AmentityName", "Sort", "SpecificInfo" },
                values: new object[,]
                {
                    { 1, "設備", null, null },
                    { 2, "服務", null, null },
                    { 3, "安全", null, null }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "ID", "AccountStatus", "Avatar", "CityName", "CountryName", "CreatedAt", "DistrictName", "EditAt", "Email", "EmergencyContact", "EmergencyNumber", "FirstName", "Job", "LastName", "Latitude", "Live", "Longitude", "Mobile", "Obsession", "Password", "Pet", "PhoneNumber", "PostalCode", "SelfIntroduction", "Street" },
                values: new object[,]
                {
                    { 1, 0, "https://picsum.photos/240/240/?random=8", "台中市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5117), "中區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5135), "leonardopikachu@smail.com", null, null, "Leonardo", "自由工作者", "Pikachu", "24.1367091", "台中市, 臺灣", "120.6807817", "0933456789", "探索世界", "12345678", "我的狐狸貓叫做寶貝", "0933456789", "40043", "是在地的台中人。喜歡到處旅遊親近自然，搜挖各地美食與文化。跟大部份喜愛旅遊的人一樣，從事美術設計與創作的事業。相逢相識即是緣分，歡迎光臨指教。", "建國路111號" },
                    { 2, 0, "https://picsum.photos/240/240/?random=2", "台北市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5149), "大安區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5150), "taylorswift@email.com", null, null, "Taylor", "自由攝影師", "Swift", "25.0418651", "台北市, 臺灣", "121.5445294", "0955778899", "攝影、烹飪", "password123", "我有一隻可愛的貓咪", "0955778899", "10651", "我是一位熱愛攝影的自由工作者,喜歡到處拍攝大自然的美景。平常也會參加一些攝影比賽,希望能藉此認識更多同好。除了攝影,我也很喜歡烹飪,經常嘗試不同國家的料理。", "忠孝東路六段200號" },
                    { 3, 0, "https://picsum.photos/240/240/?random=5", "高雄市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5157), "鼓山區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5158), "davidlee@mail.com", null, null, "小智", "教師", "林", "22.6402174", "高雄市, 臺灣", "120.2690626", "0987654321", "戶外運動、旅遊", "qwertyui", "我沒有寵物", "0987654321", "80449", "我是一位熱愛戶外運動的教師,平常假日我都會安排一些戶外活動,像是健行、爬山或是騎自行車。我也很喜歡分享旅遊的經驗,希望能藉此結交更多志同道合的夥伴。", "鼓山區鹽埕區287號" },
                    { 4, 0, "https://picsum.photos/240/240/?random=7", "台南市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5164), "中西區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5165), "sophiawang@gmail.com", null, null, "春嬌", "作家", "余", "22.9952354", "台南市, 臺灣", "120.2095524", "0912345678", "閱讀、寫作", "bookworm", "我有一隻可愛的貴賓狗", "0912345678", "70041", "我是一位熱愛閱讀的文學工作者,平常除了撰寫作品之外,也會參加一些讀書會或是文學講座。我很喜歡認識不同領域的人,互相交流想法和經驗。", "中西區民生路二段86號" },
                    { 5, 0, "https://picsum.photos/240/240/?random=3", "新竹市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5172), "東區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5173), "michaelchen@yahoo.com", null, null, "志明", "會計師", "張", "24.8050914", "新竹市, 臺灣", "120.9705871", "0976543210", "音樂、旅遊", "singerslife", "我有一隻可愛的哈士奇", "0976543210", "30076", "我是一位熱愛音樂的業餘歌手,平常會參加一些歌唱比賽或是在小酒吧駐場演出。除了音樂之外,我也很喜歡旅遊,希望能透過旅行認識更多不同的文化。", "東區光復路二段235號" }
                });

            migrationBuilder.InsertData(
                table: "RoomCategories",
                columns: new[] { "ID", "Icon", "RoomCategory", "Sort" },
                values: new object[,]
                {
                    { 1, "<span class=\"material-symbols-outlined\">apartment</span>", "公寓 Apartment", 1 },
                    { 2, "<span class=\"material-symbols-outlined\">house</span>", "獨棟 House", 2 },
                    { 3, "<span class=\"material-symbols-outlined\">night_shelter</span>", "家庭式 Home", 3 },
                    { 4, "<span class=\"material-symbols-outlined\">bedroom_parent</span>", "精品 Luxury", 4 },
                    { 5, "<span class=\"material-symbols-outlined\">home_and_garden</span>", "莊園 Garden", 5 },
                    { 6, "<span class=\"material-symbols-outlined\">villa</span>", "民宿 BNB", 6 },
                    { 7, "<span class=\"material-symbols-outlined\">cabin</span>", "小木屋 Cabin", 7 },
                    { 8, "<span class=\"material-symbols-outlined\">camping</span>", "帳篷 Camp", 8 },
                    { 9, "<span class=\"material-symbols-outlined\">airport_shuttle</span>", "露營車CamperVan", 9 }
                });

            migrationBuilder.InsertData(
                table: "LanguageSpeaker",
                columns: new[] { "ID", "Language", "MemberID" },
                values: new object[,]
                {
                    { 1, 0, 1 },
                    { 2, 1, 1 },
                    { 3, 0, 2 },
                    { 4, 2, 2 },
                    { 5, 0, 3 },
                    { 6, 3, 3 },
                    { 7, 0, 4 },
                    { 8, 4, 4 },
                    { 9, 0, 5 },
                    { 10, 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "RoomAmentityCategories",
                columns: new[] { "ID", "AmentityName", "AmentityTypeID", "Icon" },
                values: new object[,]
                {
                    { 1, "Wifi", 1, "<span class=\"material-symbols-outlined\">wifi</span>" },
                    { 2, "電視", 1, "<span class=\"material-symbols-outlined\">tv</span>" },
                    { 3, "廚房", 1, "<span class=\"material-symbols-outlined\">cooking</span>" },
                    { 4, "洗衣機", 1, "<span class=\"material-symbols-outlined\">local_laundry_service</span>" },
                    { 5, "室內免費停車", 1, "<span class=\"material-symbols-outlined\">local_parking</span>" },
                    { 6, "空調設備", 1, "<span class=\"material-symbols-outlined\">ac_unit</span>" },
                    { 7, "游泳池", 2, "<span class=\"material-symbols-outlined\">pool</span>" },
                    { 8, "按摩浴缸", 2, "<span class=\"material-symbols-outlined\">hot_tub</span>" },
                    { 9, "庭院", 2, "<span class=\"material-symbols-outlined\">outdoor_garden</span>" },
                    { 10, "烤肉區", 2, "<span class=\"material-symbols-outlined\">outdoor_grill</span>" },
                    { 11, "戶外用餐區", 2, "<span class=\"material-symbols-outlined\">deck</span>" },
                    { 12, "火坑", 2, "<span class=\"material-symbols-outlined\">local_fire_department</span>" },
                    { 13, "煙霧警報器", 3, "<span class=\"material-symbols-outlined\">detector_smoke</span>" },
                    { 14, "急救包", 3, "<span class=\"material-symbols-outlined\">medical_services</span>" },
                    { 15, "一氧化碳警報器", 3, "<span class=\"material-symbols-outlined\">detector_alarm</span>" },
                    { 16, "滅火器", 3, "<span class=\"material-symbols-outlined\">fire_extinguisher</span>" },
                    { 17, "監視錄影器", 3, "<span class=\"material-symbols-outlined\">motion_sensor_ative</span>" },
                    { 18, "武器", 3, "<span class=\"material-symbols-outlined\">swords</span>" },
                    { 19, "危險動物", 3, "<span class=\"material-symbols-outlined\">pets</span>" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "ID", "BathroomCount", "BedCount", "BedroomCount", "CheckInEndTime", "CheckInStartTime", "CheckOutTime", "CityName", "CountryName", "CreatedAt", "DistrictName", "EditedAt", "FixedPrice", "GuestCount", "IsDelete", "Latitude", "LocationDesription", "Longitude", "MemberID", "NearyByTrasportation", "Note", "PostalCode", "Review", "RoomCategoryID", "RoomDescription", "RoomName", "RoomStatus", "Sort", "Street", "WeekendPrice" },
                values: new object[,]
                {
                    { 1, 2, 4, 2, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5400), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5399), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5401), "台中市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5395), "中區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5396), 9453m, 4, false, "24.1367091", "如何到達高樓景觀公寓?<br>台中火車站：搭乘300~310任何一號的公車於科博館站下車，車程約10分鐘", "120.6807817", 2, "16號小築高樓景觀公寓為短期月租套房", null, "40043", null, 1, "乾淨 整潔 簡約 舒適 溫馨 雙人套房 ,房間乾淨清雅,每個房間皆是溫馨的和室木地板.有32吋液晶電視,小冰箱,冷氣,衛浴設備,距離火車站徒步約8分鐘,住宿地方徒步5分鐘有\"宮原眼科\"冰品 綠川廊道景點,想逛美食\"一中商圈\"徒步約15分鐘  整條皆為可吃可逛的不夜城。<br>*附近有收費停車場，停車方便。附近有多線公車直達-逢甲夜市-東海大學-梧棲魚港-彩虹眷村-高美濕地---南投縣、日月潭-埔里-清境農場-溪頭-衫林溪----鹿港小鎮@多處景點。<br><br>#請留意~入住前需先提供您的身份証或健保卡圖供大樓作登記，如不方便提供者請勿訂房，謝謝。<br>###有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房、有潔癖要求完美無瑕疵者請勿訂房<br>很重要所以說三次，謝謝🙏", "台中縱橫四海 躺著睡 橫著睡 讓你睡上癮的住宿首選（不挑房）", 2, null, "建國路111號", null },
                    { 2, 1, 1, 1, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5417), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5416), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5418), "台北市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5414), "大安區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5415), 2800m, 2, false, "25.0418651", "從捷運忠孝新生站出口步行約5分鐘即可抵達", "121.5445294", 3, "捷運忠孝新生站步行5分鐘", null, "10651", null, 1, "寬敞明亮的空間,採光良好,室內設計簡約時尚,提供高品質的住宿體驗。客房配備有舒適的雙人床、32吋液晶電視、小型冰箱和無線網路。浴室乾濕分離,備有淋浴設備和沐浴用品。距離捷運站僅步行5分鐘,周邊有許多美食及購物景點。<br><br>適合情侶、朋友或家庭入住,是您在台北市區短期居住的理想選擇。", "台北悠閒悅居 寧靜舒適的都會渡假體驗", 1, null, "忠孝東路六段200號", null },
                    { 3, 4, 8, 5, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5429), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5428), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5430), "高雄市", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5426), "鼓山區", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5427), 25000m, 10, false, "22.6402174", "從高雄市區開車約30分鐘可抵達", "120.2690626", 2, "自駕車最方便", null, "80449", null, 3, "獨棟別墅佔地寬廣,室內裝潢現代典雅,戶外備有私人泳池及庭園。客房採用一流設備,提供極致的舒適體驗。親臨此處,遠離城市喧囂,盡情放鬆身心,感受慵懶的渡假氣氛。<br><br>別墅內有多間獨立空調客房,可供家庭或多人入住。周邊環境清幽雅緻,鄰近海邊及知名景點,是您rendered度假的上佳選擇。", "高雄海景渡假別墅 優閒時光的私密度假勝地", 1, null, "鼓山區鹽埕區287號", null },
                    { 4, 3, 6, 4, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5444), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5443), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5445), "宜蘭縣", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5440), "員山鄉", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5441), 18000m, 8, false, "24.7807806", "詳細位置請洽會館服務人員", "121.7316414", 5, "會館提供接駁交通工具", null, "26942", null, 3, "會館坐落於翠綠山林間,四周環境遼闊寂靜,室內裝潢採用木質元素,充滿質樸自然的渡假氛圍。提供多種房型,可供家庭或多人入住。室內設施一應俱全,客房寬敞舒適,讓您在此盡情放鬆。<br><br>會館內備有高級餐廳及SPA水療中心,戶外有大片庭園及泳池,無論是安排戶外活動或純粹放空靜心,均是理想之選。", "宜蘭villa渡假會館 環抱大自然的世外桃源", 1, null, "員山鄉錦西村16號", null },
                    { 5, 2, 4, 3, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5453), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5453), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5454), "花蓮縣", "臺灣", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5451), "秀林鄉", new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5451), 22000m, 6, false, "23.9719939", "詳細地點請洽villa服務人員", "121.5924542", 2, "villa會提供接送服務", null, "97858", null, 3, "隱身於花蓮深山中的villa,室內裝潢別出心裁,結合當代簡約元素與原木質感,呈現獨特的居住體驗。客房寬敞明亮,戶外環境清幽怡人,提供多種房型,適合家庭或多人入住。<br><br>周邊環境毗鄰山林溪流,會館內還設有私人大眾池、SPA水療等設施,讓您盡情放鬆身心,享受大自然的芬多精。", "花蓮秘境villa 深山裡的夢幻度假屋", 1, null, "秀林鄉富世村28號", null }
                });

            migrationBuilder.InsertData(
                table: "Wishlist",
                columns: new[] { "ID", "MemberID", "WishlistName" },
                values: new object[,]
                {
                    { 1, 1, "彭于晏住我隔壁房" },
                    { 2, 2, "今晚不想回家系列" },
                    { 3, 1, "說走就走~ 我有的是時間" },
                    { 4, 3, "給我海闊天空的景色" },
                    { 5, 2, "初戀粉色系" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "ID", "CheckIn", "CheckOut", "CreatedAt", "CustomerCount", "MemberID", "Note", "OrderNo", "OrderStatus", "PaymentMethod", "ReceiptNo", "RoomID", "TotalPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5230), new DateTime(2024, 4, 3, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5234), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5244), 2, 2, null, "OMG000C01", 4, 1, "AE000NA01", 1, 10000m, null },
                    { 2, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5296), new DateTime(2024, 4, 5, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5297), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5299), 3, 1, null, "OMG000C02", 4, 1, "AE000NA02", 2, 20000m, null },
                    { 3, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5303), new DateTime(2024, 4, 6, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5304), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5305), 4, 3, null, "OMG000C03", 4, 1, "AE000NA03", 3, 30000m, null },
                    { 4, new DateTime(2024, 4, 5, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5308), new DateTime(2024, 4, 8, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5309), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5310), 4, 3, null, "OMG000C03", 1, 1, "AE000NA03", 3, 30000m, null },
                    { 5, new DateTime(2024, 4, 7, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5313), new DateTime(2024, 4, 11, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5314), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5315), 2, 5, null, "OMG000C04", 1, 1, "AE000NA04", 4, 40000m, null },
                    { 6, new DateTime(2024, 4, 9, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5317), new DateTime(2024, 4, 13, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5318), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5319), 3, 4, null, "OMG000C05", 1, 1, "AE000NA05", 5, 50000m, null },
                    { 7, new DateTime(2024, 4, 9, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5321), new DateTime(2024, 4, 13, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5322), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5324), 3, 2, null, "OMG000C05", 1, 1, "AE000NA05", 5, 50000m, null },
                    { 8, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5326), new DateTime(2024, 4, 13, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5327), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5328), 3, 2, null, "OMG000C05", 1, 1, "AE000NA05", 5, 50000m, null },
                    { 9, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5330), new DateTime(2024, 4, 13, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5331), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5332), 3, 2, null, "OMG000C05", 2, 1, "AE000NA05", 5, 50000m, null },
                    { 10, new DateTime(2024, 4, 3, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5336), new DateTime(2024, 4, 13, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5337), new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5338), 3, 2, null, "OMG000C05", 3, 1, "AE000NA05", 5, 50000m, null }
                });

            migrationBuilder.InsertData(
                table: "RoomAmenities",
                columns: new[] { "ID", "AmentityID", "RoomID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 7, 1 },
                    { 3, 13, 1 },
                    { 4, 2, 2 },
                    { 5, 8, 2 },
                    { 6, 14, 2 },
                    { 7, 3, 3 },
                    { 8, 9, 3 },
                    { 9, 15, 3 },
                    { 10, 4, 4 },
                    { 11, 10, 4 },
                    { 12, 16, 4 },
                    { 13, 5, 5 },
                    { 14, 11, 5 },
                    { 15, 17, 5 }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "ID", "ImageUrl", "RoomID", "Sort" },
                values: new object[,]
                {
                    { 1, "https://picsum.photos/600/900/?random=1", 1, 1 },
                    { 2, "https://picsum.photos/900/600/?random=2", 1, 2 },
                    { 3, "https://picsum.photos/900/600/?random=3", 1, 3 },
                    { 4, "https://picsum.photos/900/600/?random=4", 1, 4 },
                    { 5, "https://picsum.photos/900/600/?random=5", 1, 5 },
                    { 6, "https://picsum.photos/600/900/?random=6", 1, 6 },
                    { 7, "https://picsum.photos/900/600/?random=7", 1, 7 },
                    { 8, "https://picsum.photos/600/900/?random=8", 2, 1 },
                    { 9, "https://picsum.photos/900/600/?random=9", 2, 2 },
                    { 10, "https://picsum.photos/900/600/?random=10", 2, 3 },
                    { 11, "https://picsum.photos/900/600/?random=11", 2, 4 },
                    { 12, "https://picsum.photos/900/600/?random=12", 2, 5 },
                    { 13, "https://picsum.photos/600/900/?random=13", 2, 6 },
                    { 14, "https://picsum.photos/900/600/?random=14", 2, 7 },
                    { 15, "https://picsum.photos/600/900/?random=15", 3, 1 },
                    { 16, "https://picsum.photos/900/600/?random=16", 3, 2 },
                    { 17, "https://picsum.photos/900/600/?random=17", 3, 3 },
                    { 18, "https://picsum.photos/900/600/?random=18", 3, 4 },
                    { 19, "https://picsum.photos/900/600/?random=19", 3, 5 },
                    { 20, "https://picsum.photos/600/900/?random=20", 3, 6 },
                    { 21, "https://picsum.photos/900/600/?random=21", 3, 7 },
                    { 22, "https://picsum.photos/600/900/?random=22", 4, 1 },
                    { 23, "https://picsum.photos/900/600/?random=23", 4, 2 },
                    { 24, "https://picsum.photos/900/600/?random=24", 4, 3 },
                    { 25, "https://picsum.photos/900/600/?random=25", 4, 4 },
                    { 26, "https://picsum.photos/900/600/?random=26", 4, 5 },
                    { 27, "https://picsum.photos/600/900/?random=27", 4, 6 },
                    { 28, "https://picsum.photos/900/600/?random=28", 4, 7 },
                    { 29, "https://picsum.photos/600/900/?random=29", 5, 1 },
                    { 30, "https://picsum.photos/900/600/?random=30", 5, 2 },
                    { 31, "https://picsum.photos/900/600/?random=31", 5, 3 },
                    { 32, "https://picsum.photos/900/600/?random=32", 5, 4 },
                    { 33, "https://picsum.photos/900/600/?random=33", 5, 5 },
                    { 34, "https://picsum.photos/600/900/?random=34", 5, 6 },
                    { 35, "https://picsum.photos/900/600/?random=35", 5, 7 },
                    { 36, "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943619/uploadFolder/pixlr-image-generator-65c4bc723adf8c016f2dcfe7_2e9fb275-6317-49f5-8888-82e68c5964d4.png", 5, 8 },
                    { 37, "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943620/uploadFolder/sample_logo_e328c061-c25a-42d1-817a-10a0ec1e723c.png", 5, 9 },
                    { 38, "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943621/uploadFolder/step1_dc783258-4498-44ec-9f66-ac2256467667.webp", 5, 10 },
                    { 39, "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943621/uploadFolder/step2_b6792034-4d9f-48a9-aa69-944e7222d785.webp", 5, 11 },
                    { 40, "https://res.cloudinary.com/dtafyx6st/image/upload/v1711943622/uploadFolder/step3_cb727d36-409a-41c5-a8cb-91bb99075777.webp", 5, 12 }
                });

            migrationBuilder.InsertData(
                table: "WishlistDetails",
                columns: new[] { "ID", "RoomID", "WishlistID" },
                values: new object[,]
                {
                    { 1, 3, 5 },
                    { 2, 2, 5 },
                    { 3, 1, 3 },
                    { 4, 2, 3 },
                    { 5, 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "RoomReviews",
                columns: new[] { "ID", "CreatedAt", "RatingScore", "ReviewContent" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5813), 5, "整潔度整體還不錯" },
                    { 2, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5817), 5, "整潔度整體還不錯" },
                    { 3, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5819), 5, "整潔度整體還不錯" },
                    { 4, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5821), 5, "整潔度整體還不錯" },
                    { 5, new DateTime(2024, 4, 1, 19, 51, 13, 889, DateTimeKind.Local).AddTicks(5823), 5, "整潔度整體還不錯" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ecpay_OrderID",
                table: "Ecpay",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSpeaker_MemberID",
                table: "LanguageSpeaker",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberThirdPartyLink_MemberID",
                table: "MemberThirdPartyLink",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberThirdPartyLink_ThirdPartyID",
                table: "MemberThirdPartyLink",
                column: "ThirdPartyID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MemberID",
                table: "Orders",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoomID",
                table: "Orders",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_AmentityID",
                table: "RoomAmenities",
                column: "AmentityID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmenities_RoomID",
                table: "RoomAmenities",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAmentityCategoryies_AmentityTypeID",
                table: "RoomAmentityCategories",
                column: "AmentityTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomImages_RoomID",
                table: "RoomImages",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPrices_RoomID",
                table: "RoomPrices",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_MemberID",
                table: "Rooms",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomCategoryID",
                table: "Rooms",
                column: "RoomCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_MemberID",
                table: "Wishlist",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistDetails_RoomID",
                table: "WishlistDetails",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistDetails_WishlistID",
                table: "WishlistDetails",
                column: "WishlistID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ecpay");

            migrationBuilder.DropTable(
                name: "LanguageSpeaker");

            migrationBuilder.DropTable(
                name: "MemberThirdPartyLink");

            migrationBuilder.DropTable(
                name: "RoomAmenities");

            migrationBuilder.DropTable(
                name: "RoomImages");

            migrationBuilder.DropTable(
                name: "RoomPrices");

            migrationBuilder.DropTable(
                name: "RoomReviews");

            migrationBuilder.DropTable(
                name: "WishlistDetails");

            migrationBuilder.DropTable(
                name: "ThirdPartyLogin");

            migrationBuilder.DropTable(
                name: "RoomAmentityCategories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "AmentityTypes");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "RoomCategories");
        }
    }
}
