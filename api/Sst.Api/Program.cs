using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sst.Api;
using Sst.Database;
using Sst.Plaid;

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
builder.Services.AddHttpClient<PlaidClient>((sp, c) =>
{
    c.BaseAddress = new Uri(sp.GetRequiredService<IOptions<PlaidClientOptions>>().Value.BaseAddress);
});

builder.Services.AddHandlers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("localhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("localhost");

// delay responses in development for a more realistic UX
if (app.Environment.IsDevelopment())
{
    app.Use(async (ctx, next) =>
    {
        await Task.Delay(1000);
        await next(ctx);
    });
}

app.UseFastEndpoints();
app.Run();