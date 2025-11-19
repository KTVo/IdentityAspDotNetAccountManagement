using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet9EFAPI.MVCS.Services._DB;

public class TestDBContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<User>? UserTokens { get; init; }
    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IMPORTANT: configures all Identity tables
        modelBuilder.Entity<User>();
        
    }
}
