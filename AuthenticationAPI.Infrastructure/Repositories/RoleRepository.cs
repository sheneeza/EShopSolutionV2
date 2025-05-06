using AuthenticationAPI.ApplicationCore.Contracts.Repositories;
using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(AuthenticationDbContext ctx) : base(ctx) { }

    public Task<Role?> GetByNameAsync(string name) =>
        _dbSet.SingleOrDefaultAsync(r => r.Name == name);
}