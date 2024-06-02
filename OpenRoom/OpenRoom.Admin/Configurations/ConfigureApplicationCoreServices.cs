using ApplicationCore.Interfaces;

using ApplicationCore.Services;
using Infrastructure.Data;

namespace OpenRoom.Admin.Configurations;

public static class ConfigureApplicationCoreService
{
    public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
       
        return services;
    }
}