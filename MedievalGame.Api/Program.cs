using FluentValidation;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateCharacterCommand).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateCharacterValidator>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IWeaponRepository, WeaponRepository>();

builder.Services.AddAutoMapper(config => config.AddMaps(typeof(MappingProfile).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    dbContext.SeedData();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var statusCode = exception switch
        {
            DomainException ex => ex.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = exception switch
        {
            ValidationsException valEx => ApiResponse<object>.ErrorResponse(
                valEx.Message,
                valEx.StatusCode
            ).WithErrors(valEx.Errors),

            DomainException domainEx => ApiResponse<object>.ErrorResponse(
                domainEx.Message,
                domainEx.StatusCode
            ),

            _ => ApiResponse<object>.ErrorResponse(
                "Internal server error",
                StatusCodes.Status500InternalServerError
            )
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    });
});

app.Run();