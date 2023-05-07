using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Secutiry;
using MediatR;
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
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); //.WithOrigins("http://localhost:3000");
                }
            );
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseMySql(
                config.GetRequiredSection("ConnectionStrings")["Default"],
                ServerVersion.Parse("10.9.5-mariadb")
            );
        });

        // public static IServiceCollection AddMediatR(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes);
        services.AddMediatR(typeof(List.Handler));
        services.AddAutoMapper(typeof(MappingProfiles));

        // Validator
        services.AddFluentValidationAutoValidation(); // add validator automatically
        services.AddValidatorsFromAssemblyContaining<Create>(); // add the service implement validator
        //Note: if there're in the same assembly only need scane once
        // services.AddValidatorsFromAssemblyContaining<Edit>(); // No need

        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();
        return services;
    }
}
