// because the this service is quit large, so I seperate from
// ApplicationServiceExtensions to here.
using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services
            .AddIdentityCore<AppUser>(opt => // It's enough to use core
            {
                // setting some requirements
                opt.Password.RequireNonAlphanumeric = false;
                // opt.User.RequireUniqueEmail = true; // we manully deal in Account Controller
            })
            .AddEntityFrameworkStores<DataContext>(); // allow AppUser can work with entity framework

        // the key need to be the same with in TokenService.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        // Bearer is an protocol inside http header like:
        // """
        // Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWV
        // fbmFtZSI6ImJvYiIsIm5hbWVpZCI6IjM1NDcyNjU2LWJjNGEtNDU5NC
        // 1hZjg5LWUwMTczM2ZmNzlhOSIsImVtYWlsIjoiYm9iQHRlc3QuY29tI
        // iwibmJmIjoxNjgzMjczNzQyLCJleHAiOjE2ODM4Nzg1NDIsImlhdCI6
        // MTY4MzI3Mzc0Mn0.5msfs-KLwWVHf-1H-16Q8q_Viq96x3Vw9ce2fEp
        // 5Ly_dvQE3whGCJjH47qjeAoVI6eH5Du-o-SP9sByEnmc6hA
        // """
        // after bearer is jwt.
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // check the singature is correct
                    IssuerSigningKey = key,
                    ValidateIssuer = false, // check whether issue from the api server
                    ValidateAudience = false,
                };
            });
        services.AddScoped<TokenService>();

        return services;
    }
}
