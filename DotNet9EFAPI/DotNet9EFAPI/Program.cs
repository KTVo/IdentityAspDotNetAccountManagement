using System.Text;
using DotNet9EFAPI.MVCS.Models._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Models.Email;
using DotNet9EFAPI.MVCS.Services._DB;
using DotNet9EFAPI.MVCS.Services._DB.AccountManagement;
using DotNet9EFAPI.MVCS.Services._DB.JWT;
using DotNet9EFAPI.MVCS.Services.Email;
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
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

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

// SETTINGS FOR IDENTITY FRAMEWORK PASSWORD RESET
builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(2));

// LOADS EMAIL SETTINGS FROM APP SETTINGS
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// App services (fix lifetimes + typed HttpClient)
builder.Services
    .AddScoped<ITokenProvider, TokenProvider>()        // was Singleton; make Scoped to avoid transient dependency issues
    .AddScoped<IUpdateAccountDetailsService, UpdateAccountDetailsService>()
    .AddScoped<IRegistrationAccountService, RegisterAccountService>()
    .AddScoped<IPasswordResetService, PasswordResetService>()
    .AddScoped<ILoginAccountService, LoginAccountService>()
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7227")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

// --- Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();

}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // IMPORTANT: enable JWT auth
app.UseAuthorization();

app.MapControllers();

// If you are using Identity minimal APIs (only if you mapped endpoints for them elsewhere):
// app.MapIdentityApi<User>();

app.Run();
