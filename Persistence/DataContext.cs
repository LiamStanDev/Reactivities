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

        // We want the entityframework core to use migrations scaffold to create the relationship
        // Form the primary key of ActivityAttendee
        // HasKey Sets the properties that make up the primary key for this entity type
        builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

        // AppUser to many Activity
        builder
            .Entity<ActivityAttendee>()
            .HasOne(aa => aa.AppUser) // 設定AppUser
            .WithMany(aa => aa.Activities) // 指定對應的many屬性 (Domain.AppUser中)
            .HasForeignKey(aa => aa.AppUserId); // 使用AppUserId作為AppUser的外鍵

        // Activity to many AppUser
        builder
            .Entity<ActivityAttendee>()
            .HasOne(aa => aa.Activity) // 設定Activity
            .WithMany(aa => aa.Attendees) // 指定對應的many屬性 (Domain.Activity中)
            .HasForeignKey(aa => aa.ActivityId); // 使用ActivityId作為外鍵
    }
}
