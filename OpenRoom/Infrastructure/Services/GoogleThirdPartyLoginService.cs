using ApplicationCore.DTO;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class GoogleThirdPartyLoginService : IGoogleThirdPartyLoginService
	{
        private readonly IRepository<Member> _memberRepo;
        private readonly IRepository<MemberThirdPartyLink> _memberThirdPartyLinkRepo;
        private readonly IRepository<ThirdPartyLogin> _thirdPartyLoginRepo;
        private readonly HttpClient _httpClient;

        public GoogleThirdPartyLoginService(IRepository<Member> memberRepo, IRepository<MemberThirdPartyLink> memberThirdPartyLinkRepo, IRepository<ThirdPartyLogin> thirdPartyLoginRepo, HttpClient httpClient)
        {
            _memberRepo = memberRepo;
            _memberThirdPartyLinkRepo = memberThirdPartyLinkRepo;
            _thirdPartyLoginRepo = thirdPartyLoginRepo;
            _httpClient = httpClient;
        }

        public async Task<GoogleUserInfo> GetUserInfoAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo?alt=json");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var googleUserInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(content);
                googleUserInfo.AccessToken = accessToken;

                return googleUserInfo;
            }
            return null;
        }

        public int CheckAndSaveUserInfo(GoogleUserInfo userInfo)
        {
            try
            {
                // 1. 檢查是否已經有該用戶的記錄
                var existingMember = _thirdPartyLoginRepo.Any(m => m.Provider == "Google" && m.ProviderUserId == userInfo.Id);
                if (existingMember == false)
                {
                    // 2. 如果沒有，創建新的用戶記錄
                    var newMember = new Member
                    {
                        // 將GoogleUserInfo的屬性對應到Member的屬性
                        FirstName = userInfo.Given_name,
                        LastName = userInfo.Family_name,
                        Email = userInfo.Email,
                        CreatedAt = DateTime.UtcNow,
                        EditAt = DateTime.UtcNow,
                        Password = "ThirdPartyLogin" + userInfo.AccessToken,
                        Avatar = userInfo.Picture,
                        AccountStatus = 1,
                    };
                    //_memberRepo.Add(newMember);                        

                    // 3. 創建第三方登入的資訊記錄
                    var newThirdPartyLogin = new ThirdPartyLogin
                    {
                        Provider = "Google",
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
                    var thirdPartyId = _thirdPartyLoginRepo.FirstOrDefault(m => m.Provider == "Google" && m.ProviderUserId == userInfo.Id).Id;
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
