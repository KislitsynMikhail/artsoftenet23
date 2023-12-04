using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Logic.Users;

public class UserContext : IdentityDbContext<User>
{
    public UserContext(DbContextOptions<UserContext> options): base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql("Host=localhost; Port=5432; Database=auth; User ID=postgres; Password=1; Pooling=true;");
    }

    public DbSet<User> ApplicationUsers => Set<User>();
}

public class User : IdentityUser
{
    public int Year { get; set; }

    public override required string Email { get; set; }
}