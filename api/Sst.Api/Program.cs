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
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
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

var app = builder.Build();

app.UseHttpsRedirection();

app.UseFastEndpoints();
app.Run();