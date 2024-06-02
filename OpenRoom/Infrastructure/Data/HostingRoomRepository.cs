using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Data
{
    public class HostingRoomRepository : EfRepository<Room>, IHostingRoomRepository
    {
        private readonly OpenRoomContext _dbContext;

        public HostingRoomRepository(OpenRoomContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }      
    }
}
