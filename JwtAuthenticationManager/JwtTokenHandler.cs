using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationAPI.ApplicationCore.Entities;
using AuthenticationAPI.ApplicationCore.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthenticationManager;

public class JwtTokenHandler
{
    public const string JWT_SECURITY_KEY = "HRfb1GlFOmfY4Uf";
    private const int JWT_TOKEN_VALIDITY_MINS = 20;

    public JwtTokenHandler()
    {
        
    }

    public AuthenticationResponse GenerateToken(AuthenticationRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return null;
        }

        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, request.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //add claim for role new Claim(ClaimTypes.Role, request.)
        });
        
        claimsIdentity.AddClaims(
            request.Roles.Select(r => new Claim(ClaimTypes.Role, r))
        );


        var signingCredential = new SigningCredentials(new SymmetricSecurityKey(tokenKey), 
            SecurityAlgorithms.HmacSha256Signature);

        var securityTokenDescriptor = new SecurityTokenDescriptor();
        securityTokenDescriptor.Subject = claimsIdentity;
        securityTokenDescriptor.Expires = tokenExpiryTimeStamp;
        securityTokenDescriptor.SigningCredentials = signingCredential;
        
        var securityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken =  securityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = securityTokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse
        {
            Username = request.Username,
            ExpiresIn = (int) tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            JwtToken = token
        };
    }
}