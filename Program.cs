using System.Diagnostics;
using System.Runtime.InteropServices;
using Guidex_Backend.Application.Interface;
using Guidex_Backend.Application.Service;
using Guidex_Backend.Infrastructure;
using Guidex_Backend.Infrastructure.Client;
using Guidex_Backend.Infrastructure.Interface;
using Guidex_Backend.Util;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IOllamaClient, OllamaClient>();
builder.Services.AddScoped<ILlmService, LlmService>();
builder.Services.AddScoped<IEmbeddingGenerator, EmbeddingGenerator384>();
builder.Services.AddScoped<IEmbeddingsAccessor, PgVectorEmbeddingsAccessor>();
builder.Services.AddScoped<IMassageHistoryLoader, InMemoryStaticMassageHistoryLoader>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Guidex-Backend API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Guidex-Backend API v1");
    });

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var baseUrl = app.Urls.FirstOrDefault() ?? "http://localhost:5000";
        TryOpenBrowser($"{baseUrl.TrimEnd('/')}/swagger");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void TryOpenBrowser(string url)
{
    try
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
            return;
        }

        Process.Start("xdg-open", url);
    }
    catch
    {
        // No-op if the environment does not support launching a browser.
    }
}
