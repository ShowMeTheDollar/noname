using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)//設定服務 //放兩個參數configuration, serviceconfiguration
        {
            var connectionString = configuration.GetConnectionString("OpenRoomDB");//方法傳一個字串進去(ConnectionString裡面寫的key)，appsetting or secret裡寫的那個東西，也可用GetSection[:]，appsetting一定要寫，找到有寫的後才會換secrect裡的
            //組態設定
            services.AddDbContext<OpenRoomContext>(options => options.UseSqlServer(configuration.GetConnectionString("OpenRoomDB")));
            //services.AddDbContext<OpenRoomContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        }
    }
}
