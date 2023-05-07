namespace Domain;

public class ActivityAttendee
{
    public string AppUserId { get; set; } // foreign key
    public AppUser AppUser { get; set; } // navigator
    public Guid ActivityId { get; set; } // foreign key
    public Activity Activity { get; set; } // navigator
    public bool IsHost { get; set; } // standalone property
}
