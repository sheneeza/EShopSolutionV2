using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationAPI.ApplicationCore.Contracts.Repositories;
using AuthenticationAPI.ApplicationCore.Contracts.Services;
using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.ApplicationCore.Models;
using JwtAuthenticationManager;

namespace AuthenticationAPI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository    _userRepo;
        private readonly IRoleRepository    _roleRepo;
        private readonly JwtTokenHandler    _jwtHandler;
        private readonly IPasswordHelper    _hasher;

        public AuthService(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            JwtTokenHandler jwtHandler,
            IPasswordHelper hasher)
        {
            _userRepo   = userRepo;
            _roleRepo   = roleRepo;
            _jwtHandler = jwtHandler;
            _hasher     = hasher;
        }

        public async Task<UserLoginResponseViewModel> LoginAsync(LoginModel model)
        {
            var user = await _userRepo
                        .GetByUsernameAsync(model.Username)
                       ?? throw new UnauthorizedAccessException("Invalid credentials");

            if (!_hasher.Verify(model.Password, user.Password, user.Salt))
                throw new UnauthorizedAccessException("Invalid credentials");

            // pull role names
            var roles = user.UserRoles
                            .Select(ur => ur.Role.Name)
                            .ToList();

            // build JWT request
            var authReq = new AuthenticationRequest {
                Username = user.Username,
                Password = model.Password,
                Roles    = roles
            };

            var authRes = _jwtHandler.GenerateToken(authReq)
                         ?? throw new Exception("Token generation failed");

            return new UserLoginResponseViewModel {
                Token     = authRes.JwtToken,
                ExpiresIn = authRes.ExpiresIn
            };
        }

        public async Task<int> RegisterAdminAsync(RegisterModel model)
        {
            if (await _userRepo.GetByUsernameAsync(model.Username) is not null)
                throw new ApplicationException("Username already exists");
            if (await _userRepo.GetByEmailAsync(model.Email) is not null)
                throw new ApplicationException("Email already exists");

            var salt = _hasher.GenerateSalt();
            var hash = _hasher.Hash(model.Password, salt);

            var user = new User {
                FirstName = model.FirstName,
                LastName  = model.LastName,
                Username  = model.Username,
                EmailId     = model.Email,
                Password  = hash,
                Salt      = salt
            };

            // attach the admin role by ID
            user.UserRoles.Add(new UserRole { RoleId = model.RoleId });

            return await _userRepo.InsertAsync(user);
        }

        public async Task<int> CustomerRegisterAsync(CustomerRegisterModel model)
        {
            if (await _userRepo.GetByEmailAsync(model.Email) is not null)
                throw new ApplicationException("Email already exists");

            var salt = _hasher.GenerateSalt();
            var hash = _hasher.Hash(model.Password, salt);

            // look up the “Customer” role by name
            var customerRole = await _roleRepo.GetByNameAsync("Customer")
                                ?? throw new KeyNotFoundException("Customer role not found");

            var user = new User {
                FirstName = model.FirstName,
                LastName  = model.LastName,
                Username  = model.Email, // using email as username
                EmailId     = model.Email,
                Password  = hash,
                Salt      = salt
            };
            user.UserRoles.Add(new UserRole { RoleId = customerRole.Id });

            return await _userRepo.InsertAsync(user);
        }

        public async Task<int> UpdateAsync(UpdateModel model)
        {
            var user = await _userRepo.GetByIdAsync(model.Id)
                       ?? throw new KeyNotFoundException("User not found");

            user.FirstName = model.FirstName;
            user.LastName  = model.LastName;
            if (!string.IsNullOrWhiteSpace(model.Email))
                user.EmailId = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var newSalt = _hasher.GenerateSalt();
                user.Salt     = newSalt;
                user.Password = _hasher.Hash(model.Password, newSalt);
            }

            return await _userRepo.UpdateAsync(user);
        }

        public Task<int> DeleteAsync(int id)
            => _userRepo.DeleteAsync(id);

        public Task<IEnumerable<User>> GetAllUsersAsync(PaginationFilter filter)
            => _userRepo.GetAllPagedAsync(filter.PageNumber, filter.PageSize);

        public Task<User?> GetUserAsync(int id)
            => _userRepo.GetByIdAsync(id);
    }
}
