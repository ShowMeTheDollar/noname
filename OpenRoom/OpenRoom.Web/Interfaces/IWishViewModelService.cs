using ApplicationCore.Entities;
using OpenRoom.Web.ViewModels;

namespace OpenRoom.Web.Interfaces
{
    public interface IWishViewModelService
    {
        WishlistViewModel GetCardList();
        WishlistDetailViewModel GetRooms(int wishlistId, int myCount);
        OrderDetailsViewModel GetOrderRoomInfo(int orderId);
        TravelViewModel GetTravelCard();
        TravelViewModel GetTravelCard(int memberId);
        List<CustomCategory> GetCustomerCategoryRooms();
        Task<ReceiptViewModel> GetReceipt(int orderId); 
    }
}