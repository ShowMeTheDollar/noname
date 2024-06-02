
using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using OpenRoom.Web.ViewModels;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OpenRoom.Web.Services.CacheServices
{
    public class RedisCacheSearchViewModelService : ISearchViewModelService
    {
        private readonly IDistributedCache _cache;
        private readonly SearchViewModelService _searchViewModelService;
        private static readonly string _searchKey = "search";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

        public RedisCacheSearchViewModelService(SearchViewModelService searchViewModelService, IDistributedCache cache)
        {
            _searchViewModelService = searchViewModelService;
            _cache = cache;
        }

        public async Task<SearchViewModel> GetAllRoomsAsync(int page, int pageSize = 12)
        {
            var key = $"{_searchKey}-{page}-{pageSize}";
            var cacheItems = ByteArrayToObj<SearchViewModel>(await _cache.GetAsync(key));
            if (cacheItems is null)
            {
                var realItems = await _searchViewModelService.GetAllRoomsAsync(page, pageSize);
                var byteArrResult = ObjectToByteArray(realItems);
                await _cache.SetAsync(key, byteArrResult, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = _defaultCacheDuration,
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
                return realItems;
            }

            return cacheItems;
        }


        public async Task<SearchViewModel> BasicSearchRoomsAsync(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory, int page, int pageSize = 12)
        {
            return await _searchViewModelService.BasicSearchRoomsAsync(location, checkOutDate, checkOutDate, numberOfGuests, roomCategory, page, pageSize);
        }

        public async Task<SearchViewModel> DetailedSearchRoomsAsync(SearchFilterDto filter, int page, int pageSize = 12)
        {
            return await _searchViewModelService.DetailedSearchRoomsAsync(filter, page, pageSize);
        }
        private byte[] ObjectToByteArray(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        private T ByteArrayToObj<T>(byte[] byteArr) where T : class
        {
            return byteArr is null ? null : JsonSerializer.Deserialize<T>(byteArr);
        }
    }
}
