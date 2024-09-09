using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities.Accounting;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

/// <summary>
/// Represents a service for creating authentication tokens.
/// </summary>
public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(config["TokenKey"]));
    private readonly UserManager<AppUser> _userManager = userManager;

    ///<inheritdoc/>
    public async Task<string> CreateToken(AppUser user)
    {
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName)
        ];

        IList<string> roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(r=> new Claim(ClaimTypes.Role, r)));

        SigningCredentials creds = new(_key,SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}