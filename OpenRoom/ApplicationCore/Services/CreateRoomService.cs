using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class CreateRoomService : ICreateRoomService
    {
        private readonly IRepository<Room> _roomRepo;
        private readonly IRepository<RoomCategory> _roomCategoryRepo;
        private readonly IRepository<RoomAmentityCategory> _roomAmentityCategoryRepo;
        private readonly IRepository<RoomAmenity> _roomAmenityRepo;
        private readonly IRepository<RoomImage> _roomImageRepo;
        private readonly ILogger<CreateRoomService> _logger;
        public CreateRoomService(IRepository<Room> roomRepo, IRepository<RoomCategory> roomCategoryRepo, IRepository<RoomAmentityCategory> roomAmentityCategoryRepo, IRepository<RoomAmenity> roomAmenityRepo, IRepository<RoomImage> roomImageRepo, ILogger<CreateRoomService> logger = null)
        {
            _roomRepo = roomRepo;
            _roomCategoryRepo = roomCategoryRepo;
            _roomAmentityCategoryRepo = roomAmentityCategoryRepo;
            _roomAmenityRepo = roomAmenityRepo;
            _roomImageRepo = roomImageRepo;
            _logger = logger;
        }

        public async Task<int> SaveRoomAsync(CreateRoomDto roomCreationDto)
        {
            try
            {
                //var memberIdStr = _userService.GetMemberId(); // 從 UserService 獲取當前登錄用戶的 memberId
                //if (string.IsNullOrEmpty(memberIdStr) || !int.TryParse(memberIdStr, out int memberId))
                //{
                //    throw new Exception("User must be logged in to create a room.");
                //}

                // 根據 DTO 建新的 Room instance

                // 首先檢查 Latitude 和 Longitude 是否有值
                if (string.IsNullOrEmpty(roomCreationDto.Address.Latitude) || string.IsNullOrEmpty(roomCreationDto.Address.Longitude))
                {
                    // 如果沒有，記錄一條錯誤消息
                    _logger.LogError("Latitude or Longitude is null or empty.");
                    // 可以選擇拋出一個異常或處理這個問題
                    throw new InvalidOperationException("Latitude and Longitude must not be null or empty.");
                }

                var room = new Room
                {
                    MemberId = roomCreationDto.MemberId, // 確保 memberId 是整數
                    RoomCategoryId = roomCreationDto.RoomCategory,
                    CountryName = roomCreationDto.Address.Country.Trim(),
                    Street = roomCreationDto.Address.StreetAddress.Trim(),
                    CityName = roomCreationDto.Address.City.Trim(),
                    DistrictName = roomCreationDto.Address.District.Trim(),
                    PostalCode = roomCreationDto.Address.PostalCode.Trim(),
                    Latitude = roomCreationDto.Address.Latitude,
                    Longitude = roomCreationDto.Address.Longitude,
                    GuestCount = roomCreationDto.GuestCount,
                    BedroomCount = roomCreationDto.Bedrooms,
                    BedCount = roomCreationDto.Beds,
                    BathroomCount = roomCreationDto.Bathrooms,
                    RoomName = roomCreationDto.RoomName.Trim(),
                    RoomDescription = roomCreationDto.RoomDescription.Trim(),
                    FixedPrice = roomCreationDto.RoomPrice,
                    CreatedAt = DateTime.UtcNow.AddHours(8),
                };

               
                await _roomRepo.AddAsync(room);
               
               // amenities
                foreach (var amenityId in roomCreationDto.Amenities)
                {
                    bool amenityExists = await _roomAmentityCategoryRepo.AnyAsync(a => a.Id == amenityId);
                    if (!amenityExists)
                    {
                        throw new Exception($"Amenity with ID {amenityId} does not exist.");
                    }

                    var roomAmenity = new RoomAmenity
                    {
                        RoomId = room.Id,
                        AmentityId = amenityId
                    };
                 await _roomAmenityRepo.AddAsync(roomAmenity);
                   
                }

                // imageUrls
                foreach (var imageUrl in roomCreationDto.ImageUrls)
                {
                    var roomImage = new RoomImage
                    {
                        ImageUrl = imageUrl,
                        RoomId = room.Id,
                    };
                await _roomImageRepo.AddAsync(roomImage);
                   
                }

                return room.Id; // Returning the ID of the newly created room
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to create a new room.", ex);
            }


        }
    }

}

