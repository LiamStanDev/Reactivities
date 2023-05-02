// because the this service is quit large, so I seperate from
// ApplicationServiceExtensions to here.
using Domain;
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
            })
            .AddEntityFrameworkStores<DataContext>(); // allow AppUser can work with entity framework
        services.AddAuthentication();
        return services;
    }
}
