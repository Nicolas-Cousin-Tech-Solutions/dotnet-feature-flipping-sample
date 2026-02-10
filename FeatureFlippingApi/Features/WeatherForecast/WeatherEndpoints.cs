using FeatureFlippingApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using WeatherForecastModel = FeatureFlippingApi.Models.WeatherForecast;
using FeatureFlagsModel = FeatureFlippingApi.Models.FeatureFlags;

namespace FeatureFlippingApi.Features.WeatherForecast;

/// <summary>
/// Weather forecast endpoints with feature flag support
/// Demonstrates Dependency Inversion Principle: Depends on IFeatureManager abstraction
/// </summary>
public static class WeatherEndpoints
{
    public static void MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/weather", GetWeatherForecast)
            .WithName("GetWeatherForecast");
    }

    private static Results<Ok<WeatherForecastModel[]>, StatusCodeHttpResult> GetWeatherForecast(
        IFeatureManager featureManager,
        ILogger<IFeatureManager> logger)
    {
        if (!featureManager.IsFeatureEnabled(nameof(FeatureFlagsModel.EnableWeatherForecast)))
        {
            logger.LogWarning("Weather forecast feature is disabled");
            return TypedResults.StatusCode(503); // Service Unavailable
        }

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecastModel(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        logger.LogInformation("Weather forecast retrieved successfully");
        return TypedResults.Ok(forecast);
    }
}
