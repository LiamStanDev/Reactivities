namespace Domain;

public class ActivityAttendee
{
    // Actually, we only need AppUserId and ActivityId
    // but adding AppUser and Activity is easy to use (no need to search by foreign key)
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }
    public bool IsHost { get; set; }
}
