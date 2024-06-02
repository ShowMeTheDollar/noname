using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
	public class SearchQueryService
	{
		private readonly OpenRoomContext _context;

		public SearchQueryService(OpenRoomContext context)
		{
			_context = context;
		}

		public async Task<List<Room>> GetAllRoomsAsync()
		{
			return await _context.Rooms
				.Where(r => r.RoomStatus == (int)RoomStatus.Listed && r.IsDelete == false)
				.AsNoTracking()
				.Include(r => r.RoomCategory)
				.Include(r => r.RoomImages)
				.ToListAsync();
		}

		public async Task<List<Room>> GetAllRoomsAsync(int page, int pageSize = 12)
		{
			return await _context.Rooms
				.Where(r => r.RoomStatus == (int)RoomStatus.Listed && r.IsDelete == false)
                .OrderBy(r => r.Id)
                .AsNoTracking()
				.Include(r => r.RoomCategory)
				.Include(r => r.RoomImages)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<List<Room>> BasicSearchRoomsAsync(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory)
		{
			var query = _context.Rooms.AsQueryable();

			query = query.Where(room => room.RoomStatus == (int)RoomStatus.Listed && room.IsDelete == false);

			if (!string.IsNullOrEmpty(location))
			{
				query = query.Where(room => room.CountryName.Contains(location) || room.CityName.Contains(location) || room.DistrictName.Contains(location) || room.Street.Contains(location));
			}

			if (checkInDate.HasValue)
			{
				query = query.Where(room => room.CheckInStartTime >= checkInDate.Value);
			}

			if (checkOutDate.HasValue)
			{
				query = query.Where(room => room.CheckInEndTime <= checkOutDate.Value);
			}

			if (numberOfGuests.HasValue)
			{
				query = query.Where(room => room.GuestCount >= numberOfGuests.Value);
			}

            if (roomCategory.HasValue)
            {
                query = query.Where(room => room.RoomCategoryId == roomCategory.Value);
            }

			query = query.AsNoTracking();

			return await query.Include(room => room.RoomCategory)
								 .Include(room => room.RoomImages)
								 .ToListAsync();
		}

        public async Task<List<Room>> BasicSearchRoomsAsync(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory, int page, int pageSize = 12)
        {
            var query = _context.Rooms.AsQueryable();

            query = query.Where(room => room.RoomStatus == (int)RoomStatus.Listed && room.IsDelete == false);

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(room => room.CountryName.Contains(location) || room.CityName.Contains(location) || room.DistrictName.Contains(location) || room.Street.Contains(location));
            }

            if (checkInDate.HasValue)
            {
                query = query.Where(room => room.CheckInStartTime >= checkInDate.Value);
            }

            if (checkOutDate.HasValue)
            {
                query = query.Where(room => room.CheckInEndTime <= checkOutDate.Value);
            }

            if (numberOfGuests.HasValue)
            {
                query = query.Where(room => room.GuestCount >= numberOfGuests.Value);
            }

            if (roomCategory.HasValue)
            {
                query = query.Where(room => room.RoomCategoryId == roomCategory.Value);
            }

            query = query.OrderBy(r => r.Id);

            query = query.AsNoTracking();

            return await query.Include(room => room.RoomCategory)
                              .Include(room => room.RoomImages)
                              .Skip((page - 1) * pageSize)
							  .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<List<Room>> DetailedSearchRoomsAsync(SearchFilterDto filter)
		{
			var query = _context.Rooms.AsQueryable();

			query = query.Where(room => room.RoomStatus == (int)RoomStatus.Listed && room.IsDelete == false);

			if (filter.CurrentMin.HasValue)
			{
				query = query.Where(r => r.FixedPrice >= filter.CurrentMin.Value);
			}

			if (filter.CurrentMax.HasValue)
			{
				query = query.Where(r => r.FixedPrice <= filter.CurrentMax.Value);
			}

			if (filter.RoomCategory.HasValue)
			{
				query = query.Where(r => r.RoomCategoryId == filter.RoomCategory.Value);
			}

			if (filter.Bedrooms.HasValue)
			{
				query = query.Where(r => r.BedroomCount >= filter.Bedrooms.Value);
			}

			if (filter.Beds.HasValue)
			{
				query = query.Where(r => r.BedCount >= filter.Beds.Value);
			}

			if (filter.Bathrooms.HasValue)
			{
				query = query.Where(r => r.BathroomCount >= filter.Bathrooms.Value);
			}

			if (filter.Amenities != null && filter.Amenities.Any())
			{
				query = query.Where(r => r.RoomAmenities.All(ra => filter.Amenities.Contains(ra.AmentityId)));
			}

			if (filter.Languages != null && filter.Languages.Any())
			{
				query = query.Where(r => r.Member.LanguageSpeakers.Any(l => filter.Languages.Contains(l.Language)));
			}

			query = query.AsNoTracking();

			return await query.Include(room => room.RoomCategory)
								 .Include(room => room.RoomImages)
								 .ToListAsync();
		}

        public async Task<List<Room>> DetailedSearchRoomsAsync(SearchFilterDto filter, int page, int pageSize = 12)
        {
            var query = _context.Rooms.AsQueryable();

            query = query.Where(room => room.RoomStatus == (int)RoomStatus.Listed && room.IsDelete == false);

            if (filter.CurrentMin.HasValue)
            {
                query = query.Where(r => r.FixedPrice >= filter.CurrentMin.Value);
            }

            if (filter.CurrentMax.HasValue)
            {
                query = query.Where(r => r.FixedPrice <= filter.CurrentMax.Value);
            }

            if (filter.RoomCategory.HasValue)
            {
                query = query.Where(r => r.RoomCategoryId == filter.RoomCategory.Value);
            }

            if (filter.Bedrooms.HasValue)
            {
                query = query.Where(r => r.BedroomCount >= filter.Bedrooms.Value);
            }

            if (filter.Beds.HasValue)
            {
                query = query.Where(r => r.BedCount >= filter.Beds.Value);
            }

            if (filter.Bathrooms.HasValue)
            {
                query = query.Where(r => r.BathroomCount >= filter.Bathrooms.Value);
            }

            if (filter.Amenities != null && filter.Amenities.Any())
            {
                query = query.Where(r => r.RoomAmenities.All(ra => filter.Amenities.Contains(ra.AmentityId)));
            }

            if (filter.Languages != null && filter.Languages.Any())
            {
                query = query.Where(r => r.Member.LanguageSpeakers.Any(l => filter.Languages.Contains(l.Language)));
            }

            query = query.OrderBy(r => r.Id);

            query = query.AsNoTracking();

            return await query.Include(room => room.RoomCategory)
                              .Include(room => room.RoomImages)
                              .Skip((page - 1) * pageSize)
							  .Take(pageSize)
                              .ToListAsync();
        }
    }
}
