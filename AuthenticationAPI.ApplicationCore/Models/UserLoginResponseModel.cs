namespace AuthenticationAPI.ApplicationCore.Models;

public class UserLoginResponseViewModel
{
    public string Token { get; set; } = null!;
    public int ExpiresIn { get; set; }
}