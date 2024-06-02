using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
	public class LineThirdPartyLoginService : ILineThirdPartyLoginService
	{
		private readonly IConfiguration _configuration;
		private readonly IRepository<Member> _memberRepo;
		private readonly IRepository<MemberThirdPartyLink> _memberThirdPartyLinkRepo;
		private readonly IRepository<ThirdPartyLogin> _thirdPartyLoginRepo;
		private readonly HttpClient _httpClient;

		public LineThirdPartyLoginService(IConfiguration configuration, IRepository<Member> memberRepo, IRepository<MemberThirdPartyLink> memberThirdPartyLinkRepo, IRepository<ThirdPartyLogin> thirdPartyLoginRepo, HttpClient httpClient)
		{
			_configuration = configuration;
			_memberRepo = memberRepo;
			_memberThirdPartyLinkRepo = memberThirdPartyLinkRepo;
			_thirdPartyLoginRepo = thirdPartyLoginRepo;
			_httpClient = httpClient;
		}

		public LineChannelInfo GetLineLoginSettings()
		{
			return new LineChannelInfo()
			{
				Channel_ID = _configuration.GetSection("LINE-Login-Setting")["Channel_ID"],
				Channel_Secret = _configuration.GetSection("LINE-Login-Setting")["Channel_Secret"],
				CallbackURL = _configuration.GetSection("LINE-Login-Setting")["CallbackURL"]
			};
		}

		public async Task<string> GetAccessTokenAsync(string code)
		{
			var channelID = GetLineLoginSettings().Channel_ID;
			var channelSecret = GetLineLoginSettings().Channel_Secret;
			var callbackURL = GetLineLoginSettings().CallbackURL;

			var token =  isRock.LineLoginV21.Utility.GetTokenFromCode(code, channelID, channelSecret, callbackURL);

			return token.id_token;
		}

		public async Task<LineUserInfo> GetUserInfoAsync(string accessToken)
		{
			var jwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);

			var userInfo = new LineUserInfo
			{
				Id = jwtSecurityToken.Subject,
				Name = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "name").Value,
				Picture = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
				Email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email").Value,
				ExpirationTime = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value
			};

			return userInfo;
		}

		public int CheckAndSaveUserInfo(LineUserInfo userInfo)
		{
			try
			{
				// 1. 檢查是否已經有該用戶的記錄
				var existingMember = _thirdPartyLoginRepo.Any(m => m.Provider == "Line" && m.ProviderUserId == userInfo.Id);
				if (existingMember == false)
				{
					// 2. 如果沒有，創建新的用戶記錄
					var newMember = new Member
					{
						// 將GoogleUserInfo的屬性對應到Member的屬性
						FirstName = userInfo.Name,						
						Email = userInfo.Email,
						CreatedAt = DateTime.UtcNow,
						EditAt = DateTime.UtcNow,
						Password = "ThirdPartyLogin" + userInfo.ExpirationTime,
						Avatar = userInfo.Picture,
						AccountStatus = 1,
					};
					//_memberRepo.Add(newMember);                        

					// 3. 創建第三方登入的資訊記錄
					var newThirdPartyLogin = new ThirdPartyLogin
					{
						Provider = "Line",
						ProviderUserId = userInfo.Id,
						Name = userInfo.Name,
						Email = userInfo.Email,
					};
					//_thirdPartyLoginRepo.Add(newThirdPartyLogin);                       

					// 4. 創建用戶與第三方帳號的關聯記錄
					var newMemberThirdPartyLink = new MemberThirdPartyLink
					{
						//MemberId = newMember.Id,
						//ThirdPartyId = newThirdPartyLogin.Id,
						Member = newMember,
						ThirdParty = newThirdPartyLogin,
					};
					_memberThirdPartyLinkRepo.Add(newMemberThirdPartyLink);

					// 回傳memberId
					var memberId = newMember.Id;
					return memberId;
				}
				else
				{
					// 如果已經有該用戶的記錄，檢查帳號狀態，回傳memberId
					var thirdPartyId = _thirdPartyLoginRepo.FirstOrDefault(m => m.Provider == "Line" && m.ProviderUserId == userInfo.Id).Id;
					var memberId = _memberThirdPartyLinkRepo.FirstOrDefault(m => m.ThirdPartyId == thirdPartyId).MemberId;
					var accountStatus = _memberRepo.FirstOrDefault(m => m.Id == memberId).AccountStatus;

					if (accountStatus != 1)
					{
						throw new Exception("帳號狀態不是啟用狀態。");
					}

					return memberId;

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
