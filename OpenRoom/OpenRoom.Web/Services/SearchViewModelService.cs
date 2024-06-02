using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OpenRoom.Web.Services
{
    public class SearchViewModelService : ISearchViewModelService
    {
        private readonly SearchQueryService _searchQueryService;

        public SearchViewModelService(SearchQueryService searchQueryService)
        {
            _searchQueryService = searchQueryService;
        }

        public async Task<SearchViewModel> GetAllRoomsAsync(int page, int pageSize = 12)
        {
            var rooms = await _searchQueryService.GetAllRoomsAsync(page);

            var totalCount = (await _searchQueryService.GetAllRoomsAsync()).Count;

            var viewModel = new SearchViewModel
            {
                SearchRoomItems = rooms.Select(room => new SearchRoomItem
                {
                    Id = room.Id,
                    RoomName = room.RoomName,
                    FixedPrice = room.FixedPrice,
                    ImgUrls = room.RoomImages.Select(img => img.ImageUrl).ToList(),
                    RoomCategory = room.RoomCategory.RoomCategory1,
                    Review = room.Review,
                    Latitude = room.Latitude,
                    Longitude = room.Longitude
                }).ToList(),
                TotalPage = (int)Math.Ceiling((double)totalCount / pageSize),
                CurrentPage = page
            };
            return viewModel;
        }

        public async Task<SearchViewModel> BasicSearchRoomsAsync(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory, int page, int pageSize = 12)
        {
            var rooms = await _searchQueryService.BasicSearchRoomsAsync(location, checkInDate, checkOutDate, numberOfGuests, roomCategory, page);

            var totalCount = (await _searchQueryService.BasicSearchRoomsAsync(location, checkInDate, checkOutDate, numberOfGuests, roomCategory)).Count;

            var viewModel = new SearchViewModel
            {
                SearchRoomItems = rooms.Select(room => new SearchRoomItem
                {
                    Id = room.Id,
                    RoomName = room.RoomName,
                    FixedPrice = room.FixedPrice,
                    ImgUrls = room.RoomImages.Select(img => img.ImageUrl).ToList(),
                    RoomCategory = room.RoomCategory.RoomCategory1,
                    Review = room.Review,
                    Latitude = room.Latitude,
                    Longitude = room.Longitude
                }).ToList(),
                TotalPage = (int)Math.Ceiling((double)totalCount / pageSize),
                CurrentPage = page
            };
            return viewModel;
        }

        public async Task<SearchViewModel> DetailedSearchRoomsAsync(SearchFilterDto filter, int page, int pageSize = 12)
        {
            var rooms = await _searchQueryService.DetailedSearchRoomsAsync(filter, page);

            var totalCount = (await _searchQueryService.DetailedSearchRoomsAsync(filter)).Count;

            var viewModel = new SearchViewModel
            {
                SearchRoomItems = rooms.Select(room => new SearchRoomItem
                {
                    Id = room.Id,
                    RoomName = room.RoomName,
                    FixedPrice = room.FixedPrice,
                    ImgUrls = room.RoomImages.Select(img => img.ImageUrl).ToList(),
                    RoomCategory = room.RoomCategory.RoomCategory1,
                    Review = room.Review,
                    Latitude = room.Latitude,
                    Longitude = room.Longitude
                }).ToList(),
                TotalPage = (int)Math.Ceiling((double)totalCount / pageSize),
                CurrentPage = page
            };
            return viewModel;
        }
    }
}
