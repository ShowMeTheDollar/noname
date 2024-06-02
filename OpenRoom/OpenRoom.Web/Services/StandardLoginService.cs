using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace OpenRoom.Web.Services
{
    public class StandardLoginService
    {
        private readonly IRepository<Member> _memberRepo;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public StandardLoginService(IRepository<Member> memberRepo, IHttpContextAccessor httpContextAccessor)
		{
			_memberRepo = memberRepo;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<bool> StandardLoginUserAsync(StandardLoginViewModel model)
        {
			try
			{
				// 1. 檢查是否已經有該用戶的記錄
				var userCheck = _memberRepo.Any(m => m.Email == model.Email &&　m.Password == model.Password.ToSHA256());
				if (userCheck == true)
				{
					// 完成一般登入
					var memberId = _memberRepo.FirstOrDefault(m => m.Email == model.Email && m.Password == model.Password.ToSHA256()).Id;


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
					// 沒有該用戶的記錄，回傳flase                                     
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
