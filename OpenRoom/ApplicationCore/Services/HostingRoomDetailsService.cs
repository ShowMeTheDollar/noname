using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class HostingRoomDetailsService
    {
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<RoomImage> _roomImgRepo;
        private readonly IRepository<RoomAmenity> _roomAmentityRepo;
        private readonly IRepository<RoomAmentityCategory> _amentityRepo;
        private readonly IRepository<RoomCategory> _roomCategoryRepo;
        private readonly IHostingQueryService _queryService;

        public HostingRoomDetailsService(IRepository<Room> roomRepo, IRepository<RoomImage> roomImgRepo = null, IRepository<RoomAmenity> roomAmentityRepo = null, IRepository<RoomAmentityCategory> amentityRepo = null, IRepository<RoomCategory> roomCategoryRepo = null, IHostingQueryService queryService = null)
        {
            _roomRepo = roomRepo;
            _roomImgRepo = roomImgRepo;
            _roomAmentityRepo = roomAmentityRepo;
            _amentityRepo = amentityRepo;
            _roomCategoryRepo = roomCategoryRepo;
            _queryService = queryService;
        }

        public List<Room> GetHostingRoomDetail(int hostingId, int roomId)
        {
            var roomSource = _roomRepo.List(r => r.MemberId == hostingId && r.Id == roomId);            
            var roomImgs = _roomImgRepo.List(ri => roomSource.Select(r => r.Id).Contains(ri.RoomId));
            var roomAmenities = _roomAmentityRepo.List(ra => roomSource.Select(r => r.Id).Contains(ra.RoomId));
            var amentities = _amentityRepo.List(a => roomAmenities.Select(ra => ra.AmentityId).Distinct().Contains(a.Id));
            var roomCategory = _roomCategoryRepo.List(r => roomSource.Select(rc => rc.RoomCategoryId).Contains(r.Id));

            foreach (var room in roomSource)
            {
                room.RoomImages = roomImgs.Where(ri => ri.RoomId == room.Id).OrderBy(ri => ri.Sort).ToList();
                room.RoomAmenities = roomAmenities.Where(ra => ra.RoomId == room.Id).Select(ra => new RoomAmenity
                {
                    Id = ra.Id,
                    AmentityId = ra.AmentityId,
                    Amentity = amentities.First(a => a.Id == ra.AmentityId)
                }).ToList();
                room.RoomCategory = roomCategory.First(rc => rc.Id == room.RoomCategoryId);

            }

            //var roomSource = _queryService.GetHostingRooms(hostingId);

            return roomSource;
        }
    }
    
}
