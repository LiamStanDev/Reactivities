namespace API.DTOs;

// DTO: Data transfer object
// use for transfer information in different process (e.g. client and server)
public class UserDto
{
    public string DisplayName { get; set; }
    public string Token { get; set; }
    public string Image { get; set; }
    public string UserName { get; set; }
}
