using AuthenticationAPI.ApplicationCore.Contracts.Repositories;
using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AuthenticationDbContext ctx) : base(ctx) { }

    public Task<User?> GetByUsernameAsync(string username) =>
        _dbSet.SingleOrDefaultAsync(u => u.Username == username);

    public Task<User?> GetByEmailAsync(string email) =>
        _dbSet.SingleOrDefaultAsync(u => u.EmailId == email);

    public async Task<IEnumerable<User>> GetAllPagedAsync(int page, int pageSize)
    {
        return await _dbSet
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
