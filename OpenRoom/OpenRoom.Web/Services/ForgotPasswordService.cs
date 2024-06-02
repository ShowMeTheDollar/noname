using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace OpenRoom.Web.Services
{
	public class ForgotPasswordService
	{
		private readonly IRepository<Member> _memberRepo;

		public ForgotPasswordService(IRepository<Member> memberRepo)
		{
			_memberRepo = memberRepo;
		}

		public async Task<bool> ConfirmMemberEmail(string email)
		{
			try
			{
				var existingMember = await _memberRepo.AnyAsync(m => m.Email == email);
				if (existingMember)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				// 如果發生錯誤，則拋出異常，讓呼叫者知道操作失敗
				throw new Exception("操作失敗。", ex);
			}
		}

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            try
            {
                var existingMember =  _memberRepo.FirstOrDefault(m => m.Email == email);

                if (existingMember != null)
                {

                    existingMember.Password = newPassword.ToSHA256();

                    // 儲存更新後的使用者資料
                    await _memberRepo.UpdateAsync(existingMember);
                    return true;
                }
                else
                {
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
