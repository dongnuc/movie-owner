using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Movie_StructrueCode.API.Middleware;
using Movie_StructureCode.API.DependencyInjection.Extensions;
using Movie_StructureCode.Application.DependencyInjection.Extensions;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Infracstructure.DependencyInjection.Extensions;
using Movie_StructureCode.Persistence.DependencyInjection.Extensions;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ── PERSISTENCE LAYER ────────────────────────────────────────────────
builder.Services.AddConfigurePersistence(builder.Configuration);

// ── IDENTITY & AUTHENTICATION ────────────────────────────────────────
builder.Services.AddDataProtection();

builder.Services.AddIdentityCore<AppUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<Movie_StructureCode.Persistence.Context.AppDbContext>()
    .AddDefaultTokenProviders();

// ── JWT & AUTHENTICATION ────────────────────────────────────────
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// ── INFRASTRUCTURE LAYER ─────────────────────────────────────────────
builder.Services.AddConfigureInfrastructure(builder.Configuration);

// ── LOGGING ──────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration).CreateLogger();

builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// ── APPLICATION LAYER ───────────────────────────────────────────────
builder.Services.AddConfigureMediatR();
builder.Services.AddConfigureAutoMapper();

// ── MIDDLEWARE ───────────────────────────────────────────────────────
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// ── PRESENTATION LAYER ───────────────────────────────────────────────
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();

// ── SWAGGER & API DOCUMENTATION ──────────────────────────────────────
builder.Services.AddSwaggerGenNewtonsoftSupport()
                .AddFluentValidationRulesToSwagger()
                .AddSwagger();

// ── API VERSIONING ───────────────────────────────────────────────────
builder.Services.AddApiVersioning(options => options.ReportApiVersions = true)
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapCarter();

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    app.ConfigureSwagger();
}

try
{
    await app.RunAsync();
    Log.Information("stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandle exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Programs { }