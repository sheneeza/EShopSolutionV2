namespace AuthenticationAPI.ApplicationCore.Models;

public class AuthenticationRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    
    public IList<string>? Roles { get; set; }
}