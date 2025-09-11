using DotNet9EFAPI.MVCS.Models.DB;
using DotNet9EFAPI.MVCS.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNet9EFAPI.MVCS.Services._DB;

public class TestDBContext : DbContext
{

    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    // INSTANTIATE C# MODELS FOR THE DATABASE(S)
    public DbSet<Item>? Items { get; set; }
    public DbSet<User>? Users { get; set; }

}
