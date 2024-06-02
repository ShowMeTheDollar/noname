using Dapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace OpenRoom.Admin.WebApi

{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoryController : ControllerBase
    {

        private readonly OpenRoomContext _context;
        private readonly string _connectionString;
        public RoomCategoryController(OpenRoomContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("OpenRoomDB");
        }

        public async Task<IActionResult> GetRoomCategoriesCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT RoomCategoryId, COUNT(*) as RoomCount
                FROM Rooms
                GROUP BY RoomCategoryId";

                var roomCategoriesCount = await connection.QueryAsync<dynamic>(sql);

                return Ok(roomCategoriesCount);
            }
        }

    }
}
