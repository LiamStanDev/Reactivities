using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

// change from DbContext to IdentityDbContext
// Note: no need to add DbSet<AppUser>
public class DataContext : IdentityDbContext<AppUser>
{
    public DbSet<Activity> Activities { get; set; }

    public DataContext(DbContextOptions options)
        : base(options) { }
}
