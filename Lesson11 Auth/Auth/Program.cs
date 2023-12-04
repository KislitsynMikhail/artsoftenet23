using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth.Logic;
using Auth.Logic.Ids4;
using Auth.Logic.Users;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

SwaggerApiAreaDescription AddDescription(OpenApiSecurityScheme authScheme, string version, string title = null,
    string description = null)
{
    return new SwaggerApiAreaDescription()
    {
        AuthScheme = authScheme,
        Version = version,
        Title = title ?? version,
        Description = description ?? title ?? version,
    };
}

void SetDescription(SwaggerGenOptions options)
{
    var descriptionList = new[]
    {
        AddDescription(new OpenApiSecurityScheme
        {
            Description =
                "AccessToken от IdentityServer ВМЕСТЕ со словом Bearer, напр. 'Bearer kM1Q0EiLCJ0eXAiOiJK...'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        }, "public", "Public docs", ""),

        AddDescription(new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "X-Api-Key",
            },
            Scheme = "X-Api-Key",
            Name = "X-Api-Key",
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Description =
                "AccessToken от IdentityServer ВМЕСТЕ со словом Bearer, напр. 'Bearer kM1Q0EiLCJ0eXAiOiJK...'",
        }, "external", "External docs", "")
    };

    foreach (var description in descriptionList)
    {
        options.AddSecurityDefinition(description.AuthScheme.Scheme, description.AuthScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                description.AuthScheme,
                new string[] { }
            },
        });
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
   
});


builder.Services.AddSwaggerGen(opeion => { SetDescription(opeion); });
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<UserContext>();

builder.Services.AddTransient<IUserValidator<User>, CustomUsernamePolicy>();
//builder.Services.AddTransient<IPasswordValidator<User>, CustomPasswordPolicy>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<UserContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IPasswordValidator<User>, CustomPasswordPolicy>();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidIssuer = AuthOptions.ISSUER,
            RequireExpirationTime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("stamp",
        policyBuilder =>
            policyBuilder.AddRequirements(
                new StampRequirement()
            ));
});

builder.Services.AddTransient<StampHandler>();
builder.Services.AddTransient<IAuthorizationHandler, StampHandler>();


// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer("Bearer", options =>
//     {
//         options.Authority = "https://localhost:5005";
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateAudience = false
//         };
//     });

#region FirstLesson

//builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");


//builder.Services.AddSingleton<IAuthorizationHandler, IsEmployeeHandler>();

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { })
    // схема аутентификации - с помощью jwt-токенов
    /*.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Events.OnMessageReceived = async value =>
        {
            // тут можно самому проваладировать токен
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,

        };
    });*/
    // подключает и конфигурирует аутентификацию с помощью куки.
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.AddAuthorization(options =>
{
    // Политика только для авторизованных пользователей
    options.AddPolicy(
        "BasicAuthentication", 
        new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser()
            .Build());
    
    // првоерка факту существования
    options.AddPolicy("RequiredValue", policy => policy.RequireClaim("Admin"));
    
    // одно требование
    options.AddPolicy("ITOnly", policy => policy.RequireClaim("Permission", "IT"));

    // несколько требований
    options.AddPolicy("SuperIT", policy => policy.RequireClaim("Permission", "IT").RequireClaim("IT"));
    
    // пользовательский вариант конфигурации
    options.AddPolicy(
        "SuperUser",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Admin")
                       || context.User.HasClaim(claim => claim.Type == "IT")
                       || context.User.IsInRole("CEO"))
    );

    options.AddPolicy("canManageProduct", 
        policyBuilder => 
            policyBuilder.AddRequirements(
                new IsAllowedToManageProductRequirement(10)
            ));
}); 

#endregion

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// вот от сюда начинается то, что нам нужно
app.UseAuthentication();
app.UseAuthorization();
//app.UseIdentityServer();
app.MapControllers();


app.Run();

public static class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123"; // ключ для шифрации

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

public class StampRequirement : IAuthorizationRequirement
{
}

public class StampHandler : AuthorizationHandler<StampRequirement>
{
    private readonly UserManager<User> _userManager;

    public StampHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        StampRequirement requirement)
    {
        var value = context.User.Claims.FirstOrDefault(f => f.Type == nameof(IdentityUser.SecurityStamp));
        if (value == null)
        {
            return;
        }
        var id = context.User.Claims.FirstOrDefault(f => f.Type == "id");
        if (id == null)
        {
            return;
        }
        var existClaim = await _userManager.FindByIdAsync(id?.Value);
        if (existClaim == null)
        {
            return;
        }
        if (existClaim.SecurityStamp == value?.Value)
        {
            context.Succeed(requirement);
        }
    }
}


/// <summary>
/// Описание API
/// </summary>
public class SwaggerApiAreaDescription
{
    /// <summary>
    /// Версия АПИ, указывается в атрибуте [ApiExplorerSettings(GroupName)]
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Название версии
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Описание версии
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Схема авторизации
    /// </summary>
    public OpenApiSecurityScheme AuthScheme { get; set; }
}


#region FirstLesson

// BasicAuthorizationAttribute.cs
public class BasicAuthorizationAttribute : AuthorizeAttribute
{
    public BasicAuthorizationAttribute()
    {
        Policy = "BasicAuthentication";
        AuthenticationSchemes = "BasicAuthentication";
    }
}

// AuthenticatedUser.cs
public class AuthenticatedUser : IIdentity
{
    public AuthenticatedUser(
        string authenticationType,
        bool isAuthenticated,
        string name)
    {
        AuthenticationType = authenticationType;
        IsAuthenticated = isAuthenticated;
        Name = name;
    }

    public string AuthenticationType { get; }

    public bool IsAuthenticated { get; }

    public string Name { get; }
}

// BasicAuthenticationHandler.cs
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    )
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
        }

        // Get authorization key
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        var authHeaderRegex = new Regex(@"Basic (.*)");

        if (!authHeaderRegex.IsMatch(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
        }

        var authBase64 =
            Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
        var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
        var authUsername = authSplit[0];
        var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

        if (authUsername != "login" || authPassword != "password")
        {
            return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
        }

        const string Issuer = "https://gov.uk";

        //
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Dima", ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.Surname, "Dima 2", ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.Country, "Russia", ClaimValueTypes.String, Issuer),
            new Claim("Hello", "world", ClaimValueTypes.String)
        };

        // кто авторизовался
        var authenticatedUser = new AuthenticatedUser(
            "BasicAuthentication",
            true,
            authUsername);

        var claimsIdentity = new ClaimsIdentity(authenticatedUser, claims);

        // записываем информацию о пользователя в 
        // ClaimsIdentity
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // результат авторизации
        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(claimsPrincipal, Scheme.Name)
            )
        );
    }
}

public class IsAllowedToManageProductRequirement : IAuthorizationRequirement
{
    public IsAllowedToManageProductRequirement(int count)
    {
    }
}

public class IsEmployeeHandler : AuthorizationHandler<IsAllowedToManageProductRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IsAllowedToManageProductRequirement requirement)
    {
        if (context.User.HasClaim(f => f.Type == "Employee"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

#endregion