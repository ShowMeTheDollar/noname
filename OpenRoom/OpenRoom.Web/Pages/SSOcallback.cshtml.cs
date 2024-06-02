using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
#region ForSignIn
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
// sample --> https://gist.github.com/isdaviddong/8c42a0e226a33131d5af6ab4514e960e
#endregion

namespace __NameSpace__.Pages
{
    public class SSOcallbackModel : PageModel
    {
        public string UserInfo = "";
        public IConfigurationSection Channel_ID, Channel_Secret, CallbackURL;
        public SSOcallbackModel(IConfiguration config)
        {
            Channel_ID = config.GetSection("LINE-Login-Setting:Channel_ID");
            Channel_Secret = config.GetSection("LINE-Login-Setting:Channel_Secret");
            CallbackURL = config.GetSection("LINE-Login-Setting:CallbackURL");
        }
        //page to get auth code from Google SSO
        public IActionResult OnGet()
        {
            //取得返回的code
            var code = HttpContext.Request.Query["code"].ToString();
            if (string.IsNullOrEmpty(code))
            {
                using (var sw = new System.IO.StreamWriter(HttpContext.Response.Body, System.Text.Encoding.UTF8))
                {
                    sw.Write("沒有正確回應code");
                    sw.Flush();
                    return new OkResult();
                }
            }

            //顯示，測試用
            //從Code取回toke
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(code,
                Channel_ID.Value,  //TODO:請更正為你自己的 client_id
               Channel_Secret.Value, //TODO:請更正為你自己的 client_secret
                CallbackURL.Value);  //TODO:請檢查此網址必須與你的LINE Login後台Call back URL相同


            //利用access_token取得用戶資料
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            //利用id_token取得Claim資料
            var JwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(token.id_token);
            var email = "";
            //如果有email
            if (JwtSecurityToken.Claims.ToList().Find(c => c.Type == "email") != null)
                email = JwtSecurityToken.Claims.First(c => c.Type == "email").Value;

            #region SignIn
            var claims = new List<Claim>
                {
                    //use email or LINE user ID as login name
                    new Claim(ClaimTypes.Name, string.IsNullOrEmpty( email ) ? user.userId:email ),
                    //use LINE displayName as FullName
                    new Claim("FullName",user.displayName),
                    new Claim(ClaimTypes.Role, "nobody"),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
            };

            HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               authProperties);
            #endregion

            //導入首頁
            return Redirect($"/Index");
        }
    }
}
