using AuthenticationAPI.ApplicationCore.Entities;

namespace AuthenticationAPI.ApplicationCore.Contracts.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}