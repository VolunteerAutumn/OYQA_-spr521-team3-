namespace MoviesCatalog.BLL.DTOs;

public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsSubscribedToNewsletter { get; set; }
}