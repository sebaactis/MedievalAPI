using FluentValidation;
using MedievalGame.Api.Middlewares;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Interfaces;
using MedievalGame.Application.Mapping;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using MedievalGame.Infraestructure.Repositories;
using MedievalGame.Infraestructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("Jwt");

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5000);
//});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = new[] { jwtSettings["Issuer"]! },
            ValidateAudience = true,
            ValidAudiences = new[] { jwtSettings["Audience"]! },
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.NoResult();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse<string>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "Invalid or expired token",
                        Errors = new[] { context.Exception.Message },
                        TraceId = context.HttpContext.TraceIdentifier
                    };

                    var json = JsonSerializer.Serialize(response);
                    return context.Response.WriteAsync(json);
                }

                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                context.HandleResponse(); // Detiene el flujo predeterminado

                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse<string>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "Authentication is required to access this resource",
                        TraceId = context.HttpContext.TraceIdentifier
                    };

                    var json = JsonSerializer.Serialize(response);
                    return context.Response.WriteAsync(json);
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// EF Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCharacterCommand).Assembly));

// Validator
builder.Services.AddValidatorsFromAssemblyContaining<CreateCharacterValidator>();

// Repositories DI
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IWeaponRepository, WeaponRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ICharacterAuditRepository, CharacterAuditRepository>();
builder.Services.AddScoped<IItemAuditRepository, ItemAuditRepository>();
builder.Services.AddScoped<IWeaponAuditRepository, WeaponAuditRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserAuditRepository, UserAuditRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// AutoMapper
builder.Services.AddAutoMapper(config => config.AddMaps(typeof(MappingProfile).Assembly));


// Logger with SeriLog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Filter.ByExcluding(logEvent =>
        logEvent.Properties.ContainsKey("SourceContext") &&
        logEvent.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore")
    )
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(le =>
            le.Properties.ContainsKey("LogType") &&
            le.Properties["LogType"].ToString() == "\"EndpointLog\""
        )
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

// Auto Migrations when app starts
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    dbContext.SeedData();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Middlewares
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();