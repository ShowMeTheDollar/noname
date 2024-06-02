using ApplicationCore.DTO;

namespace OpenRoom.Web.Services
{
    public interface ISearchViewModelService
    {
        Task<SearchViewModel> BasicSearchRoomsAsync(string location, DateTime? checkInDate, DateTime? checkOutDate, int? numberOfGuests, int? roomCategory, int page, int pageSize = 12);
        Task<SearchViewModel> DetailedSearchRoomsAsync(SearchFilterDto filter, int page, int pageSize = 12);
        Task<SearchViewModel> GetAllRoomsAsync(int page, int pageSize = 12);
    }
}