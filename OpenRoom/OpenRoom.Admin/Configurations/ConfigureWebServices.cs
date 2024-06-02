using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using OpenRoom.Admin.Services;

namespace OpenRoom.Admin.Configurations;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<UserMangerService>();
        return services;
    }
}