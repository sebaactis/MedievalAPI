using FluentValidation;
using MedievalGame.Api.Middlewares;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Mapping;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using MedievalGame.Infraestructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


// Auto Migrations when app starts
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    dbContext.SeedData();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Middlewares
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();