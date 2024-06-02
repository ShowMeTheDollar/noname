using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Services
{
    public class CreateRoomServiceTest : ICreateRoomService
    { 
        private readonly IUnitOfWork _unitOfWork;
        



        // Constructor injection for the repositories and user service
        public CreateRoomServiceTest(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
           
        }

        public async Task<int> SaveRoomAsync(CreateRoomDto roomCreationDto) // Method changed to be synchronous
        {
            // Retrieve and validate the memberId
            //var memberId = _userService.GetMemberId();
            //if (string.IsNullOrEmpty(memberId) || !int.TryParse(memberId, out int memberIdInt))
            //{
            //    throw new InvalidOperationException("Member ID is invalid or not authenticated.");
            //}
            //roomCreationDto.MemberId = memberIdInt; // Ensure the DTO has the correct memberId

            // Begin transaction
            //_unitOfWork.Begin(); 
            if (roomCreationDto == null)
            {
                throw new ArgumentNullException(nameof(roomCreationDto), "Room creation data cannot be null.");
            }
            try
            {
                await _unitOfWork.BeginAsync();
                // Repositories from the unit of work
                var roomRepo = _unitOfWork.GetRepository<Room>();
               

                var roomAmenityRepo = _unitOfWork.GetRepository<RoomAmenity>();
                var roomAmenityCategoryRepo = _unitOfWork.GetRepository<RoomAmentityCategory>();
                var roomImageRepo = _unitOfWork.GetRepository<RoomImage>();

                // Directly using roomCreationDto.RoomCategory since it represents the ID
                var roomCategoryId = roomCreationDto.RoomCategory;
                // Verify the RoomCategory exists in the DB
                var roomCategoryRepo = _unitOfWork.GetRepository<RoomCategory>();
                var roomCategoryExists = await roomCategoryRepo.AnyAsync(rc => rc.Id == roomCategoryId);
                if (!roomCategoryExists)
                {
                    throw new KeyNotFoundException($"Room category with ID {roomCategoryId} not found.");
                }


                // Create the Room entity with the RoomCategory ID from the DTO
                var room = new Room
                {
                    
                    MemberId = roomCreationDto.MemberId, // Ensure the room is associated with the member
                    RoomName = roomCreationDto.RoomName.Trim(),
                    RoomDescription = roomCreationDto.RoomDescription.Trim(),
                    GuestCount = roomCreationDto.GuestCount,
                    BedroomCount = roomCreationDto.Bedrooms,
                    BedCount = roomCreationDto.Beds,
                    BathroomCount = roomCreationDto.Bathrooms,
                    FixedPrice = roomCreationDto.RoomPrice,
                    RoomCategoryId = roomCategoryId, // Directly assigning from DTO
                    CountryName = roomCreationDto.Address.Country,
                    CityName = roomCreationDto.Address.City,
                    Street = roomCreationDto.Address.StreetAddress,
                    DistrictName = roomCreationDto.Address.District,
                    PostalCode = roomCreationDto.Address.PostalCode,
                    Latitude = roomCreationDto.Address.Latitude,
                    Longitude = roomCreationDto.Address.Longitude,
                    CreatedAt = DateTime.UtcNow
                };          
                roomRepo.Add(room);
               
                // Handle RoomAmenityCategory and RoomAmenity
                foreach (var amenityId in roomCreationDto.Amenities)
                {
                    var roomAmenity = new RoomAmenity
                    {
                        RoomId = room.Id,
                        AmentityId = amenityId
                    };
                   
                    roomAmenityRepo.Add(roomAmenity);
                }

                // Handle RoomImage
                foreach (var imageUrl in roomCreationDto.ImageUrls)
                {
                    var roomImage = new RoomImage
                    {
                        ImageUrl = imageUrl,
                        RoomId = room.Id
                    };
                    
                    roomImageRepo.Add(roomImage);
                }

                // Commit transaction
               await _unitOfWork.CommitAsync(); // Assuming an async commit method
               return room.Id;
                //_unitOfWork.Commit(); // Commit the transaction
                //// After all operations and transaction commit
                //return Task.FromResult(room.Id); // Wrap the result in a Task
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                //_unitOfWork.Rollback(); // Rollback the transaction in case of an error
                // Handle the exception
                // For example, log the exception and rethrow or return an error code/message
                throw new Exception("Failed to create a new room.", ex);
            }
        }

    }
    
}
