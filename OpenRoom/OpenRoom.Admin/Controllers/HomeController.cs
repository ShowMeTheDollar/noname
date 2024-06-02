using OpenRoom.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dapper;
using OpenRoom.Admin.ViewModels;
using Microsoft.Data.SqlClient;
using Infrastructure.Data;


namespace OpenRoom.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly OpenRoomContext _context;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, OpenRoomContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

 
        public IActionResult Dashboard()
        {
            var connectionString = _configuration.GetConnectionString("OpenRoomDB");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string 'OpenRoomContext' is not initialized.");
            }
            var totalCountModel = new DashboardViewModel();
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    _logger.LogError("Database connection failed: {Message}", ex.Message);
                    throw;
                }

                totalCountModel.RoomCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Rooms");
                totalCountModel.OrderCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Orders");
                totalCountModel.MemberCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Members");
                totalCountModel.SalesTotal = connection.Query<decimal>(
                    "SELECT SUM(TotalPrice) FROM Orders WHERE OrderStatus IN (1, 4)").FirstOrDefault();
            }
            return View(totalCountModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Charts()
        {
            return View();
        }

        public IActionResult Echarts()
        {
            return View();
        }
        
        public IActionResult MemberList()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult WishList()
        {
            return View();
        }
        public IActionResult RoomSourceList()
        {
            return View();
        }
        public IActionResult RoomCharts()
        {
            return View();
        }

        public IActionResult WishCharts()
        {
            return View();
        }

        public IActionResult OrderCharts()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}