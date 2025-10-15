using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.Identity;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.MVCS.Services._DB.Identity;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.MVCS.Services.Email;
using DotNet9EFAPI.MVCS.Services.Identity;
using DotNet9EFAPI.MVCS.Services.REST;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- Config: JWT ---
string? jwtIssuer = builder.Configuration["Jwt:Issuer"];
string? jwtKey     = builder.Configuration["Jwt:Secret"];
string? jwtAudience= builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtIssuer))  throw new Exception("Jwt:Issuer was not provided.");
if (string.IsNullOrWhiteSpace(jwtKey))     throw new Exception("Jwt:Secret was not provided.");
if (string.IsNullOrWhiteSpace(jwtAudience))throw new Exception("Jwt:Audience was not provided.");

// --- Services ---
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<TestDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (single approach; do NOT also call AddIdentityApiEndpoints)
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

// AuthN / AuthZ
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ClockSkew                = TimeSpan.Zero,
        ValidIssuer              = jwtIssuer,
        ValidAudience            = jwtAudience,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

// App services (fix lifetimes + typed HttpClient)
builder.Services
    .AddScoped<ITokenProvider, TokenProvider>()        // was Singleton; make Scoped to avoid transient dependency issues
    .AddScoped<IIdentityUserService, IdentityUserService>()
    .AddScoped<ISendGridService, SendGridService>()
    .AddScoped<ISmtpEmailService, SmtpEmailService>();

// Typed HttpClient for RestService
builder.Services.AddHttpClient<IRestService, RestService>(client =>
{
    var baseUrl = builder.Configuration["Rest:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
        // You can set default headers here if needed
        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
});

var app = builder.Build();

// --- Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // IMPORTANT: enable JWT auth
app.UseAuthorization();

app.MapControllers();

// If you are using Identity minimal APIs (only if you mapped endpoints for them elsewhere):
// app.MapIdentityApi<User>();

app.Run();
