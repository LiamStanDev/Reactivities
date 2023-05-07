using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string Bio { get; set; }

    // add navigator
    public ICollection<ActivityAttendee> Activities { get; set; }
}
