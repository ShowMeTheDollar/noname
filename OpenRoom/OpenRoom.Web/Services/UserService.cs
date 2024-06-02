using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;

namespace OpenRoom.Web.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IRepository<Member> _memberRepo;

		public UserService(IHttpContextAccessor httpContextAccessor, IRepository<Member> memberRepo)
		{
			_httpContextAccessor = httpContextAccessor;		
			_memberRepo = memberRepo;
		}

		//public string GetMemberId()
		//{
		//    var userPrincipal = _httpContextAccessor.HttpContext.User;

		//    if (userPrincipal == null || !userPrincipal.Identity.IsAuthenticated)
		//    {
		//        return null; // 如果用戶未登入，返回null
		//    }

		//    var memberIdClaim = userPrincipal.FindFirst(claim => claim.Type == "memberId");
		//    return memberIdClaim?.Value;
		//}

		public int? GetMemberId()
		{
			var userPrincipal = _httpContextAccessor.HttpContext.User;

			if (userPrincipal == null || !userPrincipal.Identity.IsAuthenticated)
			{
				return null; // 如果用戶未登入，返回null
			}

			var memberIdClaim = userPrincipal.FindFirst(claim => claim.Type == "memberId");
			if (memberIdClaim == null)
			{
				return null; // 如果找不到memberId宣告，返回null
			}

			if (int.TryParse(memberIdClaim.Value, out int memberId))
			{
				return memberId; // 如果成功轉換，返回轉換後的整數
			}
			else
			{
				return null; // 如果轉換失敗，返回null
			}
		}

		public string GetMemberName()
        {
            var memberId = GetMemberId();
            if (memberId == null)
            {
                return null;
            }

			// 從DB拿Member
			var member = _memberRepo.SingleOrDefault(m => m.Id == memberId);

			if (member != null)
			{
				return $"{member.FirstName} {member.LastName}".Trim();
			}

			return null;
        }
    }
}
