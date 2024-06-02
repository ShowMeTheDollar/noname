using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using OpenRoom.Web.Configurations;
using Microsoft.Extensions.Caching.Memory;
using OpenRoom.Web.Helpers;
using OpenRoom.Web.Interfaces;
using OpenRoom.Web.Services;
using ApplicationCore.Interfaces;
using Infrastructure.Services;
using Infrastructure.Data;
using ApplicationCore.DTO;
using Microsoft.AspNetCore.Authentication.Google;
using System.Diagnostics.Eventing.Reader;
using System.Configuration;
#nullable disable
namespace OpenRoom.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

            Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
            builder.Services.AddApplicationCoreServices();
            //所有View Model的DI註冊請放AddViewModelServices方法中(此方法在Configurations資料夾內的ConfigureApplicationCoreServices.cs中)
            builder.Services.AddWebServices();
            builder.Services.AddSwaggerGen();
            string CorsPolicyName = "_CorsPolicy";

            builder.Services.AddCors(options =>
            {
                //1.開放所有Origins存取
                options.AddPolicy(name: CorsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            //分散式快取，Redis
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
                options.InstanceName = "MyRedisCache";
            });

            //Google & FB & Line
            builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;				
			})
			.AddCookie(options =>
            {
                options.LoginPath = "/Login/Index";
            })
            .AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
			{
				options.ClientId = builder.Configuration.GetSection("FacebookKeys:ClientId").Value;
				options.ClientSecret = builder.Configuration.GetSection("FacebookKeys:ClientSecret").Value;
				options.SaveTokens = true;
			})
			.AddGoogle(options =>
			{
				options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
				options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
				options.SaveTokens = true;
			});
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				
				app.UseHsts();
			}
			else
			{
                app.UseSwagger();
                app.UseSwaggerUI();
            }

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

            // Line
            app.UseCookiePolicy();

            app.UseCors(CorsPolicyName);

            app.UseAuthentication();
			app.UseAuthorization();

			// 下面真真新增
			//app.MapControllerRoute(
			//name: "auth",
			//pattern: "auth",
			//defaults: new { controller = "Login", action = "LineAuth" });

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");


			app.Run();
		}
	}
}
