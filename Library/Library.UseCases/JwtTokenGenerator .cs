using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.UseCases;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenGenerator(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser applicationUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, applicationUser.Id),
            new(ClaimTypes.Name, applicationUser.UserName!),
            new(ClaimTypes.Email, applicationUser.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userRoles = await _userManager.GetRolesAsync(applicationUser);
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));


        var securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? string.Empty));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}