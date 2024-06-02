using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IWishCardQueryService
    {
        List<Wishlist> GetCardList(int memberId);
        List<WishlistDetail> GetRooms(int wishlistId);
        List<Order> GetOrderRoomInfo(int memberId); 
        List<Order> GetTravelCard(int memberId);
        Task<Order> GetReceipt(int orderId);
    }
}
