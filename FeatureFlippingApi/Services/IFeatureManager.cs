namespace FeatureFlippingApi.Services;

/// <summary>
/// Interface for managing feature flags (Interface Segregation Principle)
/// </summary>
public interface IFeatureManager
{
    /// <summary>
    /// Checks if a feature is enabled
    /// </summary>
    /// <param name="featureName">Name of the feature to check</param>
    /// <returns>True if the feature is enabled, false otherwise</returns>
    bool IsFeatureEnabled(string featureName);
}
