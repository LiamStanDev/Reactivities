using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy(
                "CorsPolicy",
                policy =>
                {
                    // policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                }
            );
        });

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        var configure = config;
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseMySql(
                configure.GetConnectionString("Default"),
                ServerVersion.Parse("10.9.5-mariadb")
            );
        });

        // public static IServiceCollection AddMediatR(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes);
        services.AddMediatR(typeof(List.Handler));
        services.AddAutoMapper(typeof(MappingProfiles));
        // services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        return services;
    }
}
