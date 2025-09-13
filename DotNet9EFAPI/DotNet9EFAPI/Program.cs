using System.Security.Principal;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<TestDBContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// ADD DEPENDENCY INJECTIONS TO IOC
builder.Services
    .AddScoped<IIdentityUserService, IdentityUserService>();

builder.Services.AddDbContext<TestDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        // configure password/lockout/etc here if you want
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TestDBContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

WebApplication app = builder.Build();

app.MapIdentityApi<User>();

// Middleware
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();


