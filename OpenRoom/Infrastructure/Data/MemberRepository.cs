using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class MemberRepository : EfRepository<Member>, IMemberRepository
    {
        private readonly OpenRoomContext _dbContext;

        public MemberRepository(OpenRoomContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
