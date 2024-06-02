using Microsoft.OpenApi.Models;

namespace OpenRoom.Admin.Configurations;

public static class ConfigureSwaggerService
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BS.DemoShopTemplate Admin API",
                Description = "描述...",
            });
                
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "描述!!!JWT Authorization header using the Bearer scheme."//swager裡面寫的描述都可自訂
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { new OpenApiSecurityScheme(){ }, new List<string>() }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            
        });
        return services;
    }
}