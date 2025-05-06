namespace AuthenticationAPI.ApplicationCore.Contracts.Repositories;

public interface IPasswordHelper
{
    string GenerateSalt();
    
    string Hash(string password, string salt);
    
    bool Verify(string password, string hash, string salt);
}