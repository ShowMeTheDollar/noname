using System.Security.Claims;
using System.Text;
using OpenRoom.Admin.Helpers;
using OpenRoom.Admin.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace OpenRoom.Admin.Configurations;

public static class ConfigureAuthServices
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        //AddSingleton，new一次後都會是同一個
        services.AddSingleton<JwtHelper>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

                var issuer = jwtSettings.Issuer;
                var signKey = jwtSettings.SignKey;

                //  確保signKey不為空
                if (string.IsNullOrEmpty(signKey))
                {
                    throw new ArgumentException("JWT signing key cannot be null or empty.");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                    NameClaimType = ClaimTypes.NameIdentifier,

                    // 透過這項宣告，就可以從 "ClaimTypes.Role" 取值，並可讓 [Authorize] 判斷角色
                    RoleClaimType = ClaimTypes.Role,

                    // 一般我們都會驗證 Issuer
                    ValidateIssuer = true,
                    ValidIssuer = issuer,//這個詞是從注入來的

                    // 通常不太需要驗證 Audience
                    ValidateAudience = false,

                    // 一般我們都會驗證 Token 的有效期間
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,//1970年，TimeSpan的定義(時間間隔/in c# 是時間差)；不加這個會有錯誤

                    // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                    ValidateIssuerSigningKey = false,//想驗證才寫true

                    //如果IssuerSigningKey有設定時，ValidateIssuerSigningKey的值一定會是true，也就是會檢查SignKey的真偽
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey))//設了這個不論上一行是true or false就會驗證
                };
            });
        return services;
    }
}