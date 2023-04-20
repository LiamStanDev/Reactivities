using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Activity
{
    [Key]
    public Guid Id { get; set; } // primary key (can be reconized by ef core)

    // [Required] // we use the Fluent validator because it's show more detail information.
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string City { get; set; }
    public string Venue { get; set; }
}
