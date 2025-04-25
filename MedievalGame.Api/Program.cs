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
builder.Services.AddScoped<IItemRepository, ItemRepository>();

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
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {   
        var logger = appError.ApplicationServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var traceId = context.TraceIdentifier;

        if (exception != null)
        {
            logger.LogError(exception, "An unhandled exception ocurred");
        }

        var statusCode = exception switch
        {
            DomainException ex => ex.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = exception switch
        {
            ValidationsException valEx => (
                valEx.StatusCode,
                ApiResponse<object>.ErrorResponse(valEx.Message, valEx.StatusCode, traceId)
                    .WithErrors(valEx.Errors)
            ),

            DomainException domainEx => (
                domainEx.StatusCode,
                ApiResponse<object>.ErrorResponse(domainEx.Message, domainEx.StatusCode, traceId)
            ),

            DbUpdateException => (
                StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("A database error occurred.", StatusCodes.Status500InternalServerError, traceId)
            ),

            TaskCanceledException => (
                StatusCodes.Status408RequestTimeout,
                ApiResponse<object>.ErrorResponse("The request timed out.", StatusCodes.Status408RequestTimeout, traceId)
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("An unexpected error occurred.", StatusCodes.Status500InternalServerError, traceId)
            )
        };

        logger.LogError(exception, "Exception caught. TraceId: {TraceId}", traceId);

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    });
});

app.Run();