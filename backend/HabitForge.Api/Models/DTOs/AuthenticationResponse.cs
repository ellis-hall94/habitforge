namespace HabitForge.Api.Models.DTOs;

public class AuthenticationResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
