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

    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

    // we need to override the method in IdentityDbContext
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // create a primary key with combine AppUserId, and ActivityId
        builder.Entity<ActivityAttendee>().HasKey(aa => new { aa.AppUserId, aa.ActivityId });
        // create the relationship with ActivityAttendee and Activity
        builder
            .Entity<ActivityAttendee>()
            .HasOne(aa => aa.Activity) // ActivityAttendee has an navigator to one activity
            .WithMany(a => a.Attendees) // Activity has an navigator to many ActivityAttendee
            .HasForeignKey(aa => aa.ActivityId); // use ActivityId as ForeignKey

        // create the relationship with ActivityAttendee and AppUser
        builder
            .Entity<ActivityAttendee>()
            .HasOne(aa => aa.AppUser) // ActivityAttendee has an navigator to AppUser
            .WithMany(u => u.Activities) // AppUser has an navigator to many ActivityAttendee
            .HasForeignKey(aa => aa.AppUserId); // use AppUserId as ForeignKey
    }
}
