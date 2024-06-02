using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.Services.RoomListingService;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.EmailServices;
using Microsoft.AspNetCore.Authentication.Facebook;
using OpenRoom.Web.Services.AccountService;
using System.Configuration;

namespace OpenRoom.Web.Configurations
{
    public static class ConfigureApplicationCoreServices
    {
        public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomPriceQueryService, RoomPriceQueryService>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IHostingRoomRepository, HostingRoomRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IHostingRoomService, HostingRoomService>();
            services.AddScoped<IWishCardQueryService, WishCardQueryService>();
            services.AddScoped<IRoomQueryService, RoomQueryService>();
            services.AddScoped<IRoomDetailsService, RoomDetailsService>();
            services.AddScoped<EcpayService, EcpayService>();   
            //Cloudinary image upload 
            services.AddScoped<IImageFileUploadService, CloudinaryService>();
            services.AddScoped<IHostingQueryService, HostingQueryService>();
            services.AddScoped<IHostingRoomOrderService, HostingRoomOrderService>();
            services.AddScoped<ICreateRoomService, CreateRoomService>();
			//Login and registration
			services.AddScoped<IGoogleThirdPartyLoginService, GoogleThirdPartyLoginService>();
			services.AddScoped<ILineThirdPartyLoginService, LineThirdPartyLoginService>();			
			services.AddScoped<RegisterService>();
			services.AddScoped<StandardLoginService>();
            services.AddScoped<ForgotPasswordService>();
            

            services.AddScoped<LineService>();
            services.AddScoped<IEmailService, MailKitService>();
;          

			return services;
        }

        

       

    }
}
