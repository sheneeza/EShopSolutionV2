namespace AuthenticationAPI.ApplicationCore.Models;

public class AuthenticationResponse
{
    public string Username { get; set; }
    public string JwtToken { get; set; }
    public int ExpiresIn { get; set; }
    public IList<string>? Roles { get; set; }
}