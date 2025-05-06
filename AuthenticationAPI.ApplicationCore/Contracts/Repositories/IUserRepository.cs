using AuthenticationAPI.ApplicationCore.Entities;

namespace AuthenticationAPI.ApplicationCore.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllPagedAsync(int page, int pageSize);
}
