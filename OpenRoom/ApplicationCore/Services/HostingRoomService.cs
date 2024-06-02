using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ApplicationCore.Services
{
    public class HostingRoomService : IHostingRoomService
    {
        //_context.Rooms
        //       .Include(r => r.RoomImages)
        //        .Include(r => r.RoomAmenities)
        //        .ThenInclude(ra => ra.Amentity)
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<RoomImage> _roomImgRepo;
        private readonly IRepository<RoomAmenity> _roomAmentityRepo;
        private readonly IRepository<RoomAmentityCategory> _amentityRepo;
        private readonly IRepository<RoomCategory> _roomCategoryRepo;
        private readonly IRepository<RoomPrice> _roomPriceRepo;
        private readonly IHostingQueryService _queryService;


        public HostingRoomService(IRepository<Room> roomRepo, IRepository<RoomImage> rommImgRepo, IRepository<RoomAmenity> roomAmentityRepo, IRepository<RoomAmentityCategory> amentityRepo, IRepository<RoomCategory> roomCategoryRepo, IHostingQueryService queryService, IRepository<RoomPrice> roomPriceRepo = null)
        {
            _roomRepo = roomRepo;
            _roomImgRepo = rommImgRepo;
            _roomAmentityRepo = roomAmentityRepo;
            _amentityRepo = amentityRepo;
            _roomCategoryRepo = roomCategoryRepo;
            _queryService = queryService;
            _roomPriceRepo = roomPriceRepo;
        }

        public List<Room> GetHostingRooms(int hostingId)
        {
            var roomSource = _roomRepo.List(r => r.MemberId == hostingId);
            var roomImgs = _roomImgRepo.List(ri => roomSource.Select(r => r.Id).Contains(ri.RoomId));
            var roomAmenities = _roomAmentityRepo.List(ra => roomSource.Select(r => r.Id).Contains(ra.RoomId));
            var amentities = _amentityRepo.List(a => roomAmenities.Select(ra => ra.AmentityId).Distinct().Contains(a.Id));
            var roomCategory = _roomCategoryRepo.List(r => roomSource.Select(rc => rc.RoomCategoryId).Contains(r.Id));
            var roomPrices = _roomPriceRepo.List(rp => roomSource.Select(r => r.Id).Contains(rp.RoomId));

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

                room.RoomPrices = roomPrices.Where(rp => rp.RoomId == room.Id).ToList();
            }

            //var roomSource = _queryService.GetHostingRooms(hostingId);

            return roomSource;
        }
        public void DeleteRoom(int roomId)
        {
            var room = _roomRepo.GetById(roomId);
            if (room != null)
            {
                // 先刪除所有關聯的 RoomPrices
                var roomPrices = _roomPriceRepo.List(rp => rp.RoomId == roomId);
                foreach (var roomPrice in roomPrices)
                {
                    _roomPriceRepo.Delete(roomPrice);
                }

                // 刪除房源圖片
                var roomImages = _roomImgRepo.List(ri => ri.RoomId == roomId);
                foreach (var roomImage in roomImages)
                {
                    _roomImgRepo.Delete(roomImage);
                }

                // 刪除房源設施
                var roomAmenities = _roomAmentityRepo.List(ra => ra.RoomId == roomId);
                foreach (var roomAmenity in roomAmenities)
                {
                    _roomAmentityRepo.Delete(roomAmenity);
                }

                // 刪除房源
                _roomRepo.Delete(room);
            }
        }
        public async Task<List<Room>> GetHostingRoomsAsync(int hostingId)
        {
            var roomSource = await _roomRepo.ListAsync(r => r.MemberId == hostingId);
            var roomIds = roomSource.Select(r => r.Id).ToList();

            var roomImgs = await _roomImgRepo.ListAsync(ri => roomIds.Contains(ri.RoomId));
            var roomAmenities = await _roomAmentityRepo.ListAsync(ra => roomIds.Contains(ra.RoomId));
            var amenityIds = roomAmenities.Select(ra => ra.AmentityId).Distinct().ToList();

            var amentities = await _amentityRepo.ListAsync(a => amenityIds.Contains(a.Id));
            var categoryIds = roomSource.Select(rc => rc.RoomCategoryId).ToList();
            var roomCategories = await _roomCategoryRepo.ListAsync(r => categoryIds.Contains(r.Id));

            foreach (var room in roomSource)
            {
                room.RoomImages = roomImgs.Where(ri => ri.RoomId == room.Id).OrderBy(ri => ri.Sort).ToList();
                room.RoomAmenities = roomAmenities.Where(ra => ra.RoomId == room.Id).Select(ra => new RoomAmenity
                {
                    Id = ra.Id,
                    AmentityId = ra.AmentityId,
                    Amentity = amentities.First(a => a.Id == ra.AmentityId)
                }).ToList();
                room.RoomCategory = roomCategories.First(rc => rc.Id == room.RoomCategoryId);
            }

            return roomSource.ToList();
        }

        public Room GetRoomDetailsById(int roomId)
        {
            // First, get the room by ID with the condition that matches the hostingId.
            // Since ListAsync returns a List, we're using FirstOrDefault to get a single item.
            var room = _roomRepo.List(r => r.Id == roomId).FirstOrDefault();
            if (room == null)
            {
                // Optionally log this situation or handle it as needed.
                return null; // Early return if no room matches the criteria.
            }


            // Attempt to load RoomImages, handling the case where none are found.
            var roomImages = _roomImgRepo.List(ri => ri.RoomId == room.Id);
            room.RoomImages = roomImages ?? new List<RoomImage>(); // Ensure the property is not null even if no images are found.

          
            var roomAmenities = _roomAmentityRepo.List(ra => ra.RoomId == roomId);
            var amentities = _amentityRepo.List(a => roomAmenities.Select(ra => ra.AmentityId).Distinct().Contains(a.Id));

            // 創建一個映射，用於快速檢查特定設施是否可用
            var availableAmenityIds = new HashSet<int>(roomAmenities.Select(ra => ra.AmentityId));
            var amenitiesViewModel = roomAmenities.Where(ra => ra.RoomId == room.Id).Select(ra => new RoomAmenity    
            {
                Id = ra.Id,
                AmentityId = ra.AmentityId,
                Amentity = amentities.First(a => a.Id == ra.AmentityId),
               
            }).ToList();
            


            // Load RoomCategory
            room.RoomCategory = _roomCategoryRepo.List(rc => rc.Id == room.RoomCategoryId).First();
           

            return room;
        }
        public IEnumerable<RoomCategory> GetAllRoomCategories()
        {
            return _roomCategoryRepo.ListAll();
        }

        public void UpdateRoom(Room room)
        {
            _roomRepo.Update(room); // Update 方法                              
        }

       
    }
}   
