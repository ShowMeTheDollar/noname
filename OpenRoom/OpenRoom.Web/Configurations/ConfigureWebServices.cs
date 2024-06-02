using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using OpenRoom.Web.Services.AccountService;
using OpenRoom.Web.Services.CacheServices;
using OpenRoom.Web.Services.OrderService;

namespace OpenRoom.Web.Configurations
{
    public static class ConfigureWebServices
    {
        
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomsViewModelService, RoomsViewModelService>();
            services.AddScoped<IWishViewModelService, WishViewModelService>();
            services.AddScoped<IHostingSourceViewModelService, HostingSourceViewModelService>();
            services.AddScoped<OrderService, OrderService>();
            services.AddScoped<IAccountViewModelService, AccountViewModelService>();
            services.AddScoped<IOrderQueryService,OrderQueryService>();   
            services.AddScoped<ISearchViewModelService, RedisCacheSearchViewModelService>();
            services.AddScoped<SearchViewModelService>();
            services.AddScoped<SearchQueryService>();
            services.AddScoped<UserService>();
            services.AddScoped<AccountManagerService>();
            services.AddScoped<IHostingRoomService, HostingRoomService>();

            return services;
        }

        

    }
}
