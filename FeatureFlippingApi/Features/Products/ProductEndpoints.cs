using FeatureFlippingApi.Models;
using FeatureFlippingApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FeatureFlippingApi.Features.Products;

/// <summary>
/// Product catalog endpoints with feature flag support
/// Demonstrates feature toggling for business features
/// </summary>
public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", GetProducts)
            .WithName("GetProducts");

        app.MapGet("/api/products/{id}", GetProductById)
            .WithName("GetProductById");
    }

    private static Results<Ok<Product[]>, StatusCodeHttpResult> GetProducts(
        IFeatureManager featureManager,
        ILogger<IFeatureManager> logger)
    {
        if (!featureManager.IsFeatureEnabled(nameof(FeatureFlags.EnableProductCatalog)))
        {
            logger.LogWarning("Product catalog feature is disabled");
            return TypedResults.StatusCode(503); // Service Unavailable
        }

        var products = new[]
        {
            new Product(1, "Laptop", 999.99m, "Electronics"),
            new Product(2, "Smartphone", 699.99m, "Electronics"),
            new Product(3, "Desk Chair", 199.99m, "Furniture"),
            new Product(4, "Monitor", 299.99m, "Electronics"),
            new Product(5, "Keyboard", 79.99m, "Accessories")
        };

        logger.LogInformation("Product catalog retrieved successfully");
        return TypedResults.Ok(products);
    }

    private static Results<Ok<Product>, NotFound, StatusCodeHttpResult> GetProductById(
        int id,
        IFeatureManager featureManager,
        ILogger<IFeatureManager> logger)
    {
        if (!featureManager.IsFeatureEnabled(nameof(FeatureFlags.EnableProductCatalog)))
        {
            logger.LogWarning("Product catalog feature is disabled");
            return TypedResults.StatusCode(503); // Service Unavailable
        }

        var products = new[]
        {
            new Product(1, "Laptop", 999.99m, "Electronics"),
            new Product(2, "Smartphone", 699.99m, "Electronics"),
            new Product(3, "Desk Chair", 199.99m, "Furniture"),
            new Product(4, "Monitor", 299.99m, "Electronics"),
            new Product(5, "Keyboard", 79.99m, "Accessories")
        };

        var product = products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            logger.LogWarning("Product with ID {ProductId} not found", id);
            return TypedResults.NotFound();
        }

        logger.LogInformation("Product {ProductId} retrieved successfully", id);
        return TypedResults.Ok(product);
    }
}
