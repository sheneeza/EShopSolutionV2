namespace AuthenticationAPI.ApplicationCore.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string EmailId { get; set; }
    public string Password { get; set; }
    
    public string Salt       { get; set; }

    // navigation to the join‐table
    public ICollection<UserRole> UserRoles { get; set; }
}