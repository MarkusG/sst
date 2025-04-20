using FastEndpoints;
using Immediate.Validations.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sst.Api;
using Sst.Api.Exceptions;
using Sst.Api.Features.ImportTransactions.Mappers;
using Sst.Api.Services;
using Sst.Database;
using Sst.Plaid;
using ProblemDetails = FastEndpoints.ProblemDetails;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints();

builder.Services.AddDbContext<SstDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
    if (!builder.Environment.IsProduction())
        options.EnableSensitiveDataLogging();
});

builder.Services.Configure<PlaidClientOptions>(builder.Configuration.GetSection(nameof(PlaidClientOptions)));
builder.Services.AddScoped<PlaidClient>();
builder.Services.AddHttpClient<PlaidClient>((sp, c) => { c.BaseAddress = new Uri(sp.GetRequiredService<IOptions<PlaidClientOptions>>().Value.BaseAddress); });

builder.Services.AddScoped<CategoryService>();
builder.Services.AddSingleton<TransactionMapperProvider>();

builder.Services.AddSstApiHandlers();
builder.Services.AddSstApiBehaviors();

builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = c =>
    {
        if (c.Exception is null)
            return;

        c.ProblemDetails = c.Exception switch
        {
            ValidationException ex => new ValidationProblemDetails(
                ex
                    .Errors
                    .GroupBy(x => x.PropertyName, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(xx => xx.ErrorMessage).ToArray(),
                        StringComparer.OrdinalIgnoreCase
                    )
            )
            {
                Status = StatusCodes.Status400BadRequest,
            },

            UnauthorizedAccessException => new()
            {
                Detail = "Access denied.",
                Status = StatusCodes.Status403Forbidden,
            },

            NotFoundException => new()
            {
                Detail = "Not found.",
                Status = StatusCodes.Status404NotFound
            },

            _ => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Detail = "An error has occurred.",
                Status = StatusCodes.Status500InternalServerError,
            },
        };

        c.HttpContext.Response.StatusCode = c.ProblemDetails.Status ?? StatusCodes.Status500InternalServerError;
    };
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseCors("localhost");

// delay responses in development for a more realistic UX
if (app.Environment.IsDevelopment())
{
    app.Use(async (ctx, next) =>
    {
        await Task.Delay(100);
        await next(ctx);
    });
}

app.UseFastEndpoints();
app.MapSstApiEndpoints();

app.Run();