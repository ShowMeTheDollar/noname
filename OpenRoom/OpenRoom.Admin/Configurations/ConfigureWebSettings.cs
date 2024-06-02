using OpenRoom.Admin.Models.Settings;
using Microsoft.Extensions.Options;

namespace OpenRoom.Admin.Configurations;

public static class ConfigureWebSettings
{
    public static IServiceCollection AddWebSettings(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SettingKey))
            .AddSingleton(setting => setting.GetRequiredService<IOptions<JwtSettings>>().Value);//AddSingleton只要一樣new一次就好；所以專案只要注入JwtSettings就可以拿到

        return services;//把appsettings裡面把設定檔做成物件，變成service可看到的
    }
}