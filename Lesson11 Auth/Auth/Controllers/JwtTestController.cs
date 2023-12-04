using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Logic.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers;

[Route("test-jwt")]
[ApiController]
public class JwtTestController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<User>> _roleManager;

    public JwtTestController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("auth")]
    public async Task<string> Get(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var claims = new List<Claim>
        {
            new("role", userName),
            new("role", "rrr"),
            new(ClaimTypes.Name, user.UserName)
        };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    [Authorize(Policy = "dfd")]
    [HttpGet("info")]
    public string GetInfo()
    {
        return string.Join("", HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role"));
    }
    
    [HttpPost("register")]
    public async Task<string> RegisterAsync()
    {
        _userManager.CreateAsync();
        return string.Join("", HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role"));
    }
}