using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using ApplicationCore.Interfaces;
using ApplicationCore.Extensions;
using System.Text.RegularExpressions;
using Infrastructure.Data;
using System.Net.Http.Headers;
using ApplicationCore.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using OpenRoom.Web.Services;



namespace OpenRoom.Web.Controllers
{
    public class LoginController : Controller
    {             
        private readonly StandardLoginService _standardLoginService;
        private readonly RegisterService _registerServices;
		private readonly ForgotPasswordService _forgotPasswordService;
		private readonly IEmailService _emailService;
		private readonly IGoogleThirdPartyLoginService _googleThirdPartyLoginService;
        private readonly ILineThirdPartyLoginService _lineThirdPartyLoginService;

		public LoginController(IGoogleThirdPartyLoginService googleThirdPartyLoginService, RegisterService registerServices, StandardLoginService standardLoginService, ILineThirdPartyLoginService lineThirdPartyLoginService, ForgotPasswordService forgotPasswordService, IEmailService emailService)
		{
			_registerServices = registerServices;
			_googleThirdPartyLoginService = googleThirdPartyLoginService;
			_standardLoginService = standardLoginService;
			_lineThirdPartyLoginService = lineThirdPartyLoginService;
			_forgotPasswordService = forgotPasswordService;
			_emailService = emailService;
		}

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {          
            return View();
        }
		
		[HttpPost]
		public async Task<IActionResult> Index(StandardLoginViewModel model)
		{
			// 驗證用戶輸入的數據
			if (ModelState.IsValid)
			{
				// 一般登入處理
				var result = await _standardLoginService.StandardLoginUserAsync(model);

				if (result)
				{
                    // 檢查是否來自下單頁, 是的話將畫面導回下單程序
                    if (TempData["OrderData"] != null)
                    {
                        return RedirectToAction("SaveOrder", "Ecpay", TempData["OrderData"]);
                    }

                    // 一般登入成功，重定向到首頁
                    TempData["LoginResult"] = "success";
					return RedirectToAction("Index", "Home");
				}
				else
				{
					// 一般登入失敗，重定向到首頁
					TempData["LoginResult"] = "failure";
					return RedirectToAction("Index", "Home");
				}
			}
			// 後端檢核失敗，返回登入表單
			return View(model);
		}

		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // 驗證用戶輸入的數據
            if (ModelState.IsValid)
            {                                
                // 處理註冊
                var result = await _registerServices.RegisterUser(model);

                if (result)
                {
					// 註冊成功，重定向到登入頁面
					TempData["LoginResult"] = "success";
					return RedirectToAction("Index", "Home");
				}
                else
                {
                    // 已經註冊過，重定向到登入頁面
                    TempData["RegisterResult"] = "failure";
                    return RedirectToAction("Index", "Login");
                }
            }
            // 後端檢核失敗，返回註冊表單
            return View(model);
        }

		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			// 檢查電子郵件是否存在於資料庫
			bool isVaild = await _forgotPasswordService.ConfirmMemberEmail(email);
			// 如果存在，則生成一個權杖並發送電子郵件
			if (isVaild)
			{
				var encryptedtoken = email.Encrypt();

                // 取得當前時間並加密
                var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var encryptedTime = currentTime.Encrypt();

                var resetLink = Url.Action("ResetPassword", "Login", new { token = encryptedtoken, time = encryptedTime }, protocol: HttpContext.Request.Scheme);
				var bodyStrFormat = string.Format(@"<h1>OpenRoom 重設密碼信</h1> 
							<p>您好，您收到這封郵件是因為您請求重設您的 OpenRoom 帳號密碼。</p>
							<p>請點擊以下鏈接重設您的密碼：</p>
							<a href='{0}'>重設密碼</a>
							<p>重設連結有效時間為一個小時，請盡速修改您的密碼。</p>
							<p>如果您沒有請求重設密碼，請忽略此郵件。</p>", resetLink);
                var reseteEmail = new EmailDto
				{
					To = email,
					Subject = "OpenRoom 重設密碼信",
					Body = bodyStrFormat
                };

				_emailService.SendEmail(reseteEmail);

                TempData["ResetPassword"] = "checkEmail";
                return RedirectToAction("Index", "Home");
			}

            TempData["ResetPassword"] = "noEmail";
            return RedirectToAction("Index", "Home");
        }

		public IActionResult ResetPassword(string token, string time)
		{
            var requestTime = DateTime.Parse(time.Decrypt());
            var currentTime = DateTime.Now;
            var timeDifference = currentTime - requestTime;
            if (timeDifference.TotalHours > 1)
            {               
                return BadRequest("重置密碼的時間已過期，請重新申請。");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string time, string newPassword)
        {
			var email = token.Decrypt();
            var requestTime = DateTime.Parse(time.Decrypt());
            var currentTime = DateTime.Now;
            var timeDifference = currentTime - requestTime;
            if (timeDifference.TotalHours > 1)
            {
                return BadRequest("重置密碼的時間已過期，請重新申請。");
            }

            var result = await _forgotPasswordService.ResetPassword(email, newPassword);

			if(result)
			{
				TempData["ResetPassword"] = "success";
				return RedirectToAction("Index", "Home");
			}
			else
			{
				TempData["ResetPassword"] = "failure";
				return RedirectToAction("Index", "Home");
			}          
        }

        public IActionResult LineLogin()
		{
			var LineChannelInfo = _lineThirdPartyLoginService.GetLineLoginSettings();

			// 構建跳轉 URL
			string URL = "https://access.line.me/oauth2/v2.1/authorize?";
			URL += "response_type=code";                           // 向 Line 官方請求授權碼
			URL += "&client_id=" + LineChannelInfo.Channel_ID;     // OpenRoom 網站的用戶ID，好讓LINE 官方正確識別和授權
			URL += "&redirect_uri=" + LineChannelInfo.CallbackURL; // 授權過後要重定向的網址
			URL += "&scope=openid%20profile%20email";              // ♥ scope 取得授權後 ,可存取的資訊範圍 ,有用戶的永久識別碼、基本個人資料、Email
			URL += "&state=abcde";
			/* state的值由 OpenRoom 網站生成
               發送授權請求( 取得用戶資訊 )時送出 state=abcde
               驗證請求   ( 用戶授權成功後 ) 收到 state=abcde
               比對是否相符合 ,以防止 CSRF( 跨站請求偽造 )攻擊
             */

			return Redirect(URL);
		}

		public async Task<IActionResult> LineResponse()
		{
			var code = HttpContext.Request.Query["code"].ToString();
			if (string.IsNullOrEmpty(code))
			{
				using (var sw = new StreamWriter(HttpContext.Response.Body, System.Text.Encoding.UTF8))
				{
					sw.Write("沒有正確回應授權碼,請聯繫OpenRoom客服");
					sw.Flush();
					return new OkResult();
				}
			}

			var accessToken = await _lineThirdPartyLoginService.GetAccessTokenAsync(code);
			
			if (!string.IsNullOrEmpty(accessToken))
			{
				var lineUserInfo = await _lineThirdPartyLoginService.GetUserInfoAsync(accessToken);
                var memberId = _lineThirdPartyLoginService.CheckAndSaveUserInfo(lineUserInfo);


				var claims = new List<Claim>
                {
	                new Claim("memberId", memberId.ToString())
                };
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties
				{
					IsPersistent = true
				};
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity),
					authProperties
				);

				TempData["LoginResult"] = "success";
			}
			else
			{
				await HttpContext.SignOutAsync();
				TempData["LoginResult"] = "failure";
			}

            // 檢查是否來自下單頁, 是的話將畫面導回下單程序
            if (TempData["OrderData"] != null)
            {
                return RedirectToAction("SaveOrder", "Ecpay", TempData["OrderData"]);
            }

            return RedirectToAction("Index", "Home");
		}

		public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var accessToken = await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access_token");

            if (!string.IsNullOrEmpty(accessToken))
            {                
                var googleUserInfo = await _googleThirdPartyLoginService.GetUserInfoAsync(accessToken);
                int memberId = _googleThirdPartyLoginService.CheckAndSaveUserInfo(googleUserInfo);

                var userPrincipal = HttpContext.User;
                var identity = new ClaimsIdentity(userPrincipal.Identity.AuthenticationType);
                identity.AddClaim(new Claim("memberId", memberId.ToString()));
                var newPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);

                TempData["LoginResult"] = "success";
            }
            else
            {
                await HttpContext.SignOutAsync();
                TempData["LoginResult"] = "failure";
            }

            // 檢查是否來自下單頁, 是的話將畫面導回下單程序
            if (TempData["OrderData"] != null)
            {
                return RedirectToAction("SaveOrder", "Ecpay", TempData["OrderData"]);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            TempData["LoginResult"] = "logout";
            return RedirectToAction("Index", "Home");
        }

		/// <summary>
		/// LineLogin : Login() version 1 by jan
		/// </summary>
		/// <returns></returns>
		//public IActionResult Login()
		//{
		//	// 獲取 LINE 登入設定
		//	var lineViewModel = _lineService.GetLogin();

		//	// 構建跳轉 URL
		//	string URL = "https://access.line.me/oauth2/v2.1/authorize?";
		//	URL += "response_type=code";                          // 向 Line 官方請求授權碼
		//	URL += "&client_id=" + lineViewModel.Channel_ID;      // OpenRoom 網站的用戶ID，好讓LINE 官方正確識別和授權
		//	URL += "&redirect_uri=" + lineViewModel.CallbackURL;  // 授權過後要重定向的網址
		//	URL += "&scope=openid%20profile%20email";             // ♥ scope 取得授權後 ,可存取的資訊範圍 ,有用戶的永久識別碼、基本個人資料、Email
		//	URL += "&state=abcde";
		//	/* state的值由 OpenRoom 網站生成
		//             發送授權請求( 取得用戶資訊 )時送出 state=abcde
		//             驗證請求   ( 用戶授權成功後 ) 收到 state=abcde
		//             比對是否相符合 ,以防止 CSRF( 跨站請求偽造 )攻擊
		//           */

		//	return Redirect(URL);
		//}
		/// <summary>
		/// LineLogin : SSOCallback() version 1 by jan
		/// </summary>
		/// <returns></returns>
		//public async Task<IActionResult> SSOCallback()
		//{
		//	var code = HttpContext.Request.Query["code"].ToString(); // 從驗證請求得到的授權碼 ,再向LINE的API發送請求,取得權杖,就能取得用戶資訊。
		//	if (string.IsNullOrEmpty(code))
		//	{
		//		// StreamWriter 有寫入文本功能的類別,第一個參數是寫入的目的地,第二個參數是指定字符編碼
		//		using (var sw = new StreamWriter(HttpContext.Response.Body, System.Text.Encoding.UTF8))
		//		{
		//			sw.Write("沒有正確回應授權碼,請聯繫OpenRoom客服");
		//			sw.Flush();             // 確保上面的訊息會被立即送達到用戶端後
		//			return new OkResult();  // 向客戶端發送 HTTP 200 OK 的回應。
		//		}
		//	}

		//	// 取得 LINE 登入設定
		//	var channelID = _lineService.GetLogin().Channel_ID;
		//	var channelSecret = _lineService.GetLogin().Channel_Secret;
		//	var callbackURL = _lineService.GetLogin().CallbackURL;

		//	// 向LINE官方API發送的請求中,需要給他授權碼 以及我們網站的channelID, channelSecret, callbackURL ,來取得權杖
		//	var token = isRock.LineLoginV21.Utility.GetTokenFromCode(code, channelID, channelSecret, callbackURL);

		//	// 用權杖向Line官方的API發送請求,取得用戶的個人資料
		//	var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);

		//	// 用取得的權杖 和Line官方進行 API 請求的認證憑證, 從Line生成的Jwt ,安全的取得 Claim
		//	var jwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token.id_token);


		//	// 從取得用戶資料的資料集合裡面去尋找email屬性, 有找到的話就取得值, 沒找到就回傳空字串
		//	var email = jwtSecurityToken.Claims.ToList().Find(c => c.Type == "email")?.Value ?? "";

		//	// 使用正則表達式去除所有符號和表情符號
		//	string cleanedName = Regex.Replace(user.displayName, @"[^\w\s]|[\uD800-\uDBFF][\uDC00-\uDFFF]", "");

		//	// 這些聲明將用於用戶在我OpenRoom網站的身份驗證和授權
		//	var claims = new List<Claim>
		//	{  
		//              // 如果有找到用戶的email就用他的email 沒找到就用他的名字
		//             new Claim(ClaimTypes.NameIdentifier, user.userId),
		//	   new Claim("Name", cleanedName),
		//	   new Claim(ClaimTypes.Name, string.IsNullOrEmpty(email) ? user.userId : email)
		//	};
		//	// ClaimsIdentity 物件包含了 claims ,用來做 CookieAuthentication 驗證。以及識別和授權用戶在網站中的行為。
		//	var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		//	var authProperties = new AuthenticationProperties();

		//	// 將用戶身份信息存儲在身份驗證 cookie 中，以實現用戶的身份驗證和登錄功能。
		//	await HttpContext.SignInAsync(
		//	   CookieAuthenticationDefaults.AuthenticationScheme,   // 指定 CookieAuthentication 身份驗證方案
		//	   new ClaimsPrincipal(claimsIdentity), // 看下面
		//	   authProperties
		//	);

		//	/*
		//           創建 ClaimsPrincipal 對象，並將 claimsIdentity 對象傳遞給他。
		//           ClaimsPrincipal 代表了一個用戶的身份，它通常包含一個或多個 ClaimsIdentity 物件。
		//           通會使用 ClaimsPrincipal 對象來代表用戶的身份，而其中的 ClaimsIdentity 對象則包含了用戶的屬性資訊。
		//           authProperties 物件用來在身份驗證 cookie 中存儲其他額外的屬性(持續性、到期時間)
		//          */

		//	// 導入首頁
		//	return Redirect("/Home/Index");
		//}


		public async Task FacebookLogin()
		{
			await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties
			{
				RedirectUri = Url.Action("FacebookResponse")
			});
		}

		public async Task<IActionResult> FacebookResponse()
		{
			var Result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

			// 取得 Access Token
			var accessToken = Result.Properties.GetTokenValue("access_token");
			if (!string.IsNullOrEmpty(accessToken))
			{
				// 獲取用戶的Google個人資料
				var fbclient = new HttpClient();
				fbclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
				var response = await fbclient.GetAsync("https://graph.facebook.com/me?fields=id,email,picture,first_name,last_name");
				// 處理從Google API返回的內容
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
				}
			}

            // 檢查是否來自下單頁, 是的話將畫面導回下單程序
            if (TempData["OrderData"] != null)
            {
                return RedirectToAction("SaveOrder", "Ecpay", TempData["OrderData"]);
            }

            return RedirectToAction("Index", "Home");
		}
	}
}
