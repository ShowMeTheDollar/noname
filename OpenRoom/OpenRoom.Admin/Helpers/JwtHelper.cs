using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OpenRoom.Admin.Models.Settings;
using Microsoft.IdentityModel.Tokens;

namespace OpenRoom.Admin.Helpers;

public class JwtHelper //此helper沒寫static是因為line11~16
{
    private readonly JwtSettings _jwtSettings; //透過注入檔拿，不能寫成static，因為要注入(下方的建構式內要new出來)；這樣寫是因為這個設定檔是注入的

    public JwtHelper(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public AuthResultDto GenerateToken(string userName, int expireMinute = 30)
    {
        var defaultRoles = new List<string> { "Admin", "Users" };//先寫預設的角色有甚麼，下面再寫方法多載
        return GenerateToken(userName, defaultRoles, expireMinute);
    }

    public AuthResultDto GenerateToken(string userName, IEnumerable<string> roles, int expireMinute = 30)
    {
        var issuer = _jwtSettings.Issuer;//發行人
        var signKey = _jwtSettings.SignKey;//key

        // 設定要加入到 JWT Token 中的聲明
        var claims = new List<Claim>();

        //使用定義的規格 https://datatracker.ietf.org/doc/html/rfc7519#section-4.1
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        // 自行擴充
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));//除了基本的可自加定義，但不要原本有註冊過的
        }

        // 宣告集合所描述的身分識別
        var userClaimsIdentity = new ClaimsIdentity(claims);

        // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

        // 用來產生數位簽章的密碼編譯演算法
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // 預留位置，適用於和已發行權杖相關的所有屬性，用來定義JWT的相關設定
        var tokenDescriptor = new SecurityTokenDescriptor()//把東西該塞的塞上去
        {
            Issuer = issuer,
            Subject = userClaimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(expireMinute),
            SigningCredentials = signingCredentials
        };

        // 用來產生JWT，那串文字產出來的
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);//產出一個token處理的人
        var serializeToken = tokenHandler.WriteToken(securityToken);//寫token，最終的樣子


        return new AuthResultDto()
        {
            Token = serializeToken,
            ExpireTime = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeSeconds(),//處理時間戳，把值帶進去用秒
        };
    }

    public class AuthResultDto //會回傳的東西
    {
        public string Token { get; set; }
        public long ExpireTime { get; set; }//到期時間會變成一串數字，timespan要夠長所以用long
    }
}