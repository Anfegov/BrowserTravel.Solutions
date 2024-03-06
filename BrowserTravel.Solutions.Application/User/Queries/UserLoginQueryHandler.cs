using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BrowserTravel.Solutions.Domain.DomainErrors;
using BrowserTravel.Solutions.Domain.Dtos;
using BrowserTravel.Solutions.Domain.Exceptions;

namespace BrowserTravel.Solutions.Application.User.Queries;
public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, UserLoginDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public UserLoginQueryHandler(UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<UserLoginDto> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new CoreException(Errors.GenericError.NotFound($"{nameof(user)}"));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Sid, user.Id),
        new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        if(!int.TryParse(_config["Jwt:Expires"], out int expires)) 
        {
            throw new CoreException(Errors.UserLoginError.ValidateExpires());
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expires),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new UserLoginDto(jwt);
    }
}
