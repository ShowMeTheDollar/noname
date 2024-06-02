using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Extensions;
using isRock.LineBot;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace OpenRoom.Web.Services
{
    public class RegisterService
    {
        private readonly IRepository<Member> _memberRepo;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RegisterService(IRepository<Member> memberRepo, IHttpContextAccessor httpContextAccessor)
		{
			_memberRepo = memberRepo;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<bool> RegisterUser(RegisterViewModel model)
        {
            try
            {
                // 1. 檢查是否已經有該用戶的記錄
                var existingMember =await _memberRepo.AnyAsync(m => m.Email == model.Email);
                if (existingMember == false)
                {
                    // 2. 如果沒有，創建新的用戶記錄
                    var newMember = new Member
                    {
                        // 將RegisterViewModel的屬性對應到Member的屬性
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Mobile = model.Mobile,
                        PhoneNumber = model.PhoneNumber,
                        CreatedAt = DateTime.UtcNow,
                        EditAt = DateTime.UtcNow,
                        Password = model.Password.ToSHA256(),                       
                        AccountStatus = 1,
                    };
                    _memberRepo.Add(newMember);

					// 完成帳號註冊，自動登入
					var memberId = newMember.Id;
					var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
					claimsIdentity.AddClaim(new Claim("memberId", memberId.ToString()));
					var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					var authenticationProperties = new AuthenticationProperties()
					{
						IsPersistent = true
					};
					await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

					return true;
                }
                else
                {
                    // 如果已經有該用戶的記錄，回傳flase                                     
                    return false;

                }
            }
            catch (Exception ex)
            {
                // 如果發生錯誤，則拋出異常，讓呼叫者知道操作失敗
                throw new Exception("操作失敗。", ex);
            }
        }
    }
}
