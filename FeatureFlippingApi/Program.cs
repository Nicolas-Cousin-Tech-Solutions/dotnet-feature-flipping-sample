using FeatureFlippingApi.Models;
using FeatureFlippingApi.Services;
using FeatureFlippingApi.Features.WeatherForecast;
using FeatureFlippingApi.Features.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

// Configure feature flags from appsettings.json
// Dependency Injection Principle: Register services with their interfaces
builder.Services.Configure<FeatureFlags>(
    builder.Configuration.GetSection(FeatureFlags.SectionName));

// Register feature manager as singleton (SOLID: Single Responsibility)
builder.Services.AddSingleton<IFeatureManager, FeatureManager>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map feature-toggled endpoints (DRY principle: endpoint logic is reusable)
app.MapWeatherEndpoints();
app.MapProductEndpoints();

// Health check endpoint (always available)
app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck");

app.Run();

