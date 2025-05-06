using AuthenticationAPI.ApplicationCore.Contracts.Services;
using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.ApplicationCore.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers;
[ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseViewModel>> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var newId = await _authService.RegisterAdminAsync(model);
            return CreatedAtAction(nameof(GetUser), new { id = newId }, null);
        }

        [HttpPost("customer-register")]
        public async Task<ActionResult> CustomerRegister([FromBody] CustomerRegisterModel model)
        {
            var newId = await _authService.CustomerRegisterAsync(model);
            return CreatedAtAction(nameof(GetUser), new { id = newId }, null);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] UpdateModel model)
        {
            await _authService.UpdateAsync(model);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            await _authService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers([FromQuery] PaginationFilter filter)
        {
            var users = await _authService.GetAllUsersAsync(filter);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _authService.GetUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }