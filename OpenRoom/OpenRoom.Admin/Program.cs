using OpenRoom.Admin.Configurations;
using Infrastructure;



namespace OpenRoom.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Dependencies.ConfigureServices(builder.Configuration, builder.Services);//Infrastructure注入DI要用的
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();//MVC框架的注入方式，讓我們在httpcontext做註冊

            builder.Services.AddWebSettings(builder.Configuration);//cool~

            builder.Services
                .AddApplicationCoreServices()
                .AddAuthServices()
                .AddSwaggerService()
                .AddWebServices();
                
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();//add this request進來跟出去response在middleware做的事
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
