using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.ApplicationCore.Models;

namespace AuthenticationAPI.ApplicationCore.Contracts.Services;

// ApplicationCore/Contracts/Services/IAuthService.cs
public interface IAuthService
{
    Task<UserLoginResponseViewModel> LoginAsync(LoginModel model);
    Task<int> RegisterAdminAsync(RegisterModel model);
    Task<int> CustomerRegisterAsync(CustomerRegisterModel model);
    Task<int> UpdateAsync(UpdateModel model);
    Task<int> DeleteAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync(PaginationFilter filter);
    Task<User?> GetUserAsync(int id);
}
