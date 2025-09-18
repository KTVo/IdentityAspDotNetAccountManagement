using System.Security.Principal;
using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.MVCS.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
// using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
string? jwtKey = builder.Configuration.GetSection("Jwt:Secret").Get<string>();
string? jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();

if (jwtIssuer == null) { throw new Exception(nameof(jwtIssuer)); }
if (jwtKey == null) { throw new Exception(nameof(jwtKey)); }

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<TestDBContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// ADD DEPENDENCY INJECTIONS TO IOC
builder.Services
    .AddSingleton<ITokenProvider, TokenProvider>()
    .AddScoped<IIdentityUserService, IdentityUserService>()
    .AddScoped<IIdentityUserService, IdentityUserService>();

builder.Services.AddDbContext<TestDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen();

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TestDBContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

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
    // app.MapScalarApiReference(); 

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();


