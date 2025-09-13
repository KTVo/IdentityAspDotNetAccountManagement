using DotNet9EFAPI.MVCS.Models._DB.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet9EFAPI.MVCS.Services._DB;

public class TestDBContext : IdentityDbContext<User, IdentityRole, string>
{
    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder); // IMPORTANT: configures all Identity tables
    //     // Optional: rename tables if you want
    //     // builder.Entity<User>().ToTable("AspNetUsers");
    //     // builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
    //     // ...etc
    // }
}
