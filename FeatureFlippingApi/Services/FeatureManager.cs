using Microsoft.Extensions.Options;
using FeatureFlippingApi.Models;

namespace FeatureFlippingApi.Services;

/// <summary>
/// Service responsible for managing feature flags
/// Single Responsibility Principle: Only manages feature flag state
/// Open/Closed Principle: Can be extended without modifying existing code
/// </summary>
public class FeatureManager : IFeatureManager
{
    private readonly FeatureFlags _featureFlags;

    public FeatureManager(IOptions<FeatureFlags> featureFlags)
    {
        ArgumentNullException.ThrowIfNull(featureFlags);
        _featureFlags = featureFlags.Value ?? throw new ArgumentNullException(nameof(featureFlags));
    }

    public bool IsFeatureEnabled(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
        {
            throw new ArgumentException("Feature name cannot be null or empty", nameof(featureName));
        }

        // Use reflection to get the property value dynamically
        var property = typeof(FeatureFlags).GetProperty(featureName);
        
        if (property == null || property.PropertyType != typeof(bool))
        {
            return false;
        }

        return (bool)(property.GetValue(_featureFlags) ?? false);
    }
}
