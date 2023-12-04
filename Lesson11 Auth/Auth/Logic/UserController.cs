using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Logic.Ids4;
using Auth.Logic.Users;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Logic;

[ApiController]
[Route("user")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<User> usrMgr, RoleManager<IdentityRole> roleManager)
    {
        _userManager = usrMgr;
        _roleManager = roleManager;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "stamp")]
    public IActionResult CheckAuth()
    {
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(nameof(IdentityUser.SecurityStamp), user.SecurityStamp),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDto user)
    {
        var appUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = user.UserName,
            Email = user.Email
        };

        var result = await _userManager.CreateAsync(appUser, user.Password);
       
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        }
        
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
        }
        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(appUser, UserRoles.User);
        }
        
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            return Ok(result);
        }
        
        return BadRequest("Пользователь не найден");
    }
    
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = AuthOptions.GetSymmetricSecurityKey();

        var token = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}

public class Response
{
    public string? Status { get; set; }
    public string? Message { get; set; }
}

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}


public class UserDto
{
    [Required]
    public string UserName { get; set; }
 
    [Required]
    [EmailAddress]
    public string Email { get; set; }
 
    [Required]
    public string Password { get; set; }
}