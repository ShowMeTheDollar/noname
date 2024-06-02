using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using OpenRoom.Web.Models;
using OpenRoom.Web.ViewModels;
using OpenRoom.Web.Interfaces;
using OpenRoom.Web.Services;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using ApplicationCore.DTO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using isRock.LineBot;
using Microsoft.AspNetCore.Authorization;

namespace OpenRoom.Web.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Authorize]
    public class WishController : Controller
    {
        private readonly IWishViewModelService _wishViewModelService;
        private readonly UserService _userService;
        private readonly OpenRoomContext _db;
        public WishController(IWishViewModelService wishViewModelService, UserService userService, OpenRoomContext db)
        {
            _wishViewModelService = wishViewModelService;
            _userService = userService;
            _db = db;
        }

       
        public IActionResult WishlistDetail(int Id, int myCount)
        {
            var cardViewModel = _wishViewModelService.GetRooms(Id, myCount);
            return View(cardViewModel);
            // todo ..
        }

        // 更改房間自定義類別的名稱  --------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPut]
        //[Route("Wishlist/Update")]
        public async Task<IActionResult> UpdateWishlistNameApi([FromBody] UpdateWishlistNameApiDto request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var memberId = _userService.GetMemberId();
                if (!memberId.HasValue)
                {
                    return Unauthorized("User must be logged in to update wishlist name.");
                }
                var data = await _db.Wishlists
                .FirstOrDefaultAsync(w => w.Id == request.WishlistId);

                if (data == null)
                {
                    return NotFound("Wishlist not found.");
                }

                data.WishlistName = request.WishlistName;

                _db.Wishlists.Update(data);

                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "Successfully updated wishlist name." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        // 取得用戶 id 再加房間到 wishDetail -----------------------------------------------------------
        public async Task<Wishlist> GetOrCreateWishlistForMember(int memberId)
        {
            var wishlist = await _db.Wishlists
                .FirstOrDefaultAsync(w => w.MemberId == memberId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    MemberId = memberId,
                };
                _db.Wishlists.Add(wishlist);
                await _db.SaveChangesAsync();
            }

            return wishlist;
        }
        // 加房間到 wishDetail -------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddRoomToWishlist(int roomId)
        {
            var memberId = _userService.GetMemberId();
            if (!memberId.HasValue)
            {
                return Unauthorized("登入後才能將房間新增至願望清單");
            }

            var wishlist = await GetOrCreateWishlistForMember(memberId.Value);

            var wishlistDetail = new WishlistDetail
            {
                WishlistId = wishlist.Id,
                RoomId = roomId,
            };

            _db.WishlistDetails.Add(wishlistDetail);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "房間已成功添加到心願單！" });
        }

        //[Route("Wishlist/List/{memberId}")]
        public IActionResult Wishlist() // ----------------------------------------------------------------------------------------------------------------------------------------
        {

            var cardViewModel = _wishViewModelService.GetCardList();
            return View(cardViewModel);
        }

        // 加新的 wishlist 自定義心願單名稱   -----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddWishlist([FromBody] AddWishDto addWishDto)
        {
            try
            {

                if (addWishDto == null)
                {
                    return BadRequest(ModelState);
                }
                var memberId = _userService.GetMemberId();
                if (memberId == null)
                {
                    return Unauthorized();
                }
                // 創建新的心願名單
                var newWishlist = new Wishlist
                {
                    MemberId = memberId.Value,
                    WishlistName = addWishDto.WishlistName
                };
                _db.Wishlists.Add(newWishlist);

                await _db.SaveChangesAsync();

                return Ok(new { success = true, wishlistId = newWishlist.Id });

                //return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        // 加新的 wishlist 自定義心願單名稱   -----------------------------------------------------------------
        [HttpGet]
        public IActionResult GetMemberWishlist()
        {
            var memberId = _userService.GetMemberId();
            if (memberId == null)
            {
                return Unauthorized();
            }
            var cardList = _wishViewModelService.GetCustomerCategoryRooms();
            return Ok(cardList);
        }

        // 加新的 wishDetail 自定義心願單名稱   --------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> AddwishlistDetail([FromBody] AddwishlistDetailDto addwishlistDetailDto)
        {

            try
            {
                var memberId = _userService.GetMemberId();
                if (!memberId.HasValue)
                {
                    return Unauthorized();
                }
                if (_db.WishlistDetails.Any(wishlist => wishlist.WishlistId == addwishlistDetailDto.WishlistId && wishlist.RoomId == addwishlistDetailDto.RoomId))
                {
                    return Ok(new { success = false, message = "此房間已重複加入" });
                }
                var wishlistDetail = new WishlistDetail
                {
                    WishlistId = addwishlistDetailDto.WishlistId,
                    RoomId = addwishlistDetailDto.RoomId,
                };
                _db.Add(wishlistDetail);

                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "發生錯誤: " + ex.Message);
            }
        }

        // 刪除 wishlist 的 自定義清單 ----------------------------------------------------------------------
        [HttpDelete]
        public async Task<IActionResult> DeleteRoomFromWishlist(int wishlistId)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 刪除外面 wishlist 1.先刪裡面的 wishDetail  2.在刪外面的wishlist 
                    var wishlist = _db.Wishlists.FirstOrDefault(w => w.Id == wishlistId);
                    if(wishlist == null)
                    {
                        return BadRequest(new { success = false, message = "找不到此願望清單，刪除失敗!" }); ;
                    }

                    var wishDetails = _db.WishlistDetails.Where(w => w.WishlistId == wishlistId );
                    _db.WishlistDetails.RemoveRange(wishDetails);
                    await _db.SaveChangesAsync();
                    
                    _db.Wishlists.Remove(wishlist);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return Ok(new { success = true, message = "房間已從願望清單中刪除！" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "發生錯誤: " + ex.Message);
                }
            }
        }

        // 刪除 wishDetail 的 自定義清單 ----------------------------------------------------------------------
        [HttpDelete]
        public async Task<IActionResult> DeleteRoomFromWishDetail(int id)
        {
            try
            {
                var wishDetail = await _db.WishlistDetails.FirstOrDefaultAsync(w => w.Id == id);

                if (wishDetail == null)
                {
                    return BadRequest(new { success = false, message = "找不到此願望清單，刪除失敗!" }); ;
                }

                var deleteWishDetail = _db.WishlistDetails.FirstOrDefault(w => w.Id == id);
                if(deleteWishDetail == null)
                {
                    return Ok(new { success = false, message = "無此物件" });
                }
                _db.WishlistDetails.Remove(deleteWishDetail);
                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "房間已從願望清單中刪除！" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "發生錯誤: " + ex.Message);
            }
        }
    }
}
