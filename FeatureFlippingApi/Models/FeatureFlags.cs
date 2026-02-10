namespace FeatureFlippingApi.Models;

public class FeatureFlags
{
    public const string SectionName = "FeatureFlags";
    
    public bool EnableWeatherForecast { get; set; }
    public bool EnableProductCatalog { get; set; }
    public bool EnableDetailedLogging { get; set; }
}
