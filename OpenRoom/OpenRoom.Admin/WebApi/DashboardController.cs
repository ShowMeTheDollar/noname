using Dapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace OpenRoom.Admin.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly OpenRoomContext _context;
        private readonly string _connectionString;

        public DashboardController(OpenRoomContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("OpenRoomDB");
        }
        /// <summary>
        /// MonthlyOrders EF的寫法
        /// </summary>
        /// <returns></returns>
        //[HttpGet("monthly-orders")]
        //public IActionResult MonthlyOrders()
        //{
        //    var currentYear = DateTime.Now.Year;
        //    var monthlyOrders = _context.Orders
        //        .Where(o => o.CreatedAt.Year == currentYear)
        //        .GroupBy(o => o.CreatedAt.Month)
        //        .Select(g => new
        //        {
        //            Month = g.Key,
        //            Orders = g.Count()
        //        })
        //        .ToList();

        //    var result = Enumerable.Range(1, 12)
        //        .Select(month => monthlyOrders.FirstOrDefault(o => o.Month == month) ?? new { Month = month, Orders = 0 })
        //        .ToList();

        //    return Ok(result);
        //}

        /// <summary>
        /// MonthlyRevenue EF的寫法
        /// </summary>
        /// <returns></returns>
        //[HttpGet("monthly-revenue")]
        //public IActionResult MonthlyRevenue()
        //{
        //    var currentYear = DateTime.Now.Year;
        //    var monthlyRevenue = _context.Orders
        //        .Where(o => o.CreatedAt.Year == currentYear)
        //        .GroupBy(o => o.CreatedAt.Month)
        //        .Select(g => new
        //        {
        //            Month = g.Key,
        //            Revenue = g.Sum(o => o.TotalPrice)
        //        })
        //        .ToList();

        //    var result = Enumerable.Range(1, 12)
        //        .Select(month => monthlyRevenue.FirstOrDefault(r => r.Month == month) ?? new { Month = month, Revenue = 0m })
        //        .ToList();

        //    return Ok(result);
        //}

        [HttpGet("monthly-orders")]
        public IActionResult MonthlyOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                    SELECT 
                        MONTH(CreatedAt) AS Month,
                        COUNT(*) AS Orders
                    FROM 
                        Orders
                    WHERE 
                        YEAR(CreatedAt) = @Year
                    GROUP BY 
                        MONTH(CreatedAt)";

                var currentYear = DateTime.Now.Year;
                var monthlyOrders = connection.Query<dynamic>(sql, new { Year = currentYear }).ToList();

                var result = Enumerable.Range(1, 12)
                    .Select(month => monthlyOrders.FirstOrDefault(o => o.Month == month) ?? new { Month = month, Orders = 0 })
                    .Select(o => new { month = o.Month, orders = o.Orders })
                    .ToList();

                return Ok(result);
            }
        }

        [HttpGet("monthly-revenue")]
        public IActionResult MonthlyRevenue()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                    SELECT 
                        MONTH(CreatedAt) AS Month,
                        SUM(TotalPrice) AS Revenue
                    FROM 
                        Orders
                    WHERE 
                        YEAR(CreatedAt) = @Year AND
                        OrderStatus IN (1, 4)
                    GROUP BY 
                        MONTH(CreatedAt)";

                var currentYear = DateTime.Now.Year;
                var monthlyRevenue = connection.Query<dynamic>(sql, new { Year = currentYear }).ToList();

                var result = Enumerable.Range(1, 12)
                    .Select(month => monthlyRevenue.FirstOrDefault(r => r.Month == month) ?? new { Month = month, Revenue = 0m })
                    .Select(o => new { month = o.Month, revenue = o.Revenue })
                    .ToList();

                return Ok(result);
            }
        }
    }
}
