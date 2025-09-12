using DotNet9EFAPI.MVCS.Models._DB;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNet9EFAPI.MVCS.Services._DB;

public class TestDBContext : DbContext
{

    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    // INSTANTIATE C# MODELS FOR THE DATABASE(S)
    public DbSet<User>? Users { get; set; }

}
