using FeatureFlippingApi.Models;
using FeatureFlippingApi.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace FeatureFlippingApi.Tests.Services;

public class FeatureManagerTests
{
    [Fact]
    public void IsFeatureEnabled_WhenFeatureIsEnabled_ReturnsTrue()
    {
        // Arrange
        var featureFlags = new FeatureFlags
        {
            EnableWeatherForecast = true,
            EnableProductCatalog = false
        };
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act
        var result = featureManager.IsFeatureEnabled(nameof(FeatureFlags.EnableWeatherForecast));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFeatureEnabled_WhenFeatureIsDisabled_ReturnsFalse()
    {
        // Arrange
        var featureFlags = new FeatureFlags
        {
            EnableWeatherForecast = true,
            EnableProductCatalog = false
        };
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act
        var result = featureManager.IsFeatureEnabled(nameof(FeatureFlags.EnableProductCatalog));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsFeatureEnabled_WhenFeatureNameIsNull_ThrowsArgumentException()
    {
        // Arrange
        var featureFlags = new FeatureFlags();
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => featureManager.IsFeatureEnabled(null!));
    }

    [Fact]
    public void IsFeatureEnabled_WhenFeatureNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var featureFlags = new FeatureFlags();
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => featureManager.IsFeatureEnabled(string.Empty));
    }

    [Fact]
    public void IsFeatureEnabled_WhenFeatureDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var featureFlags = new FeatureFlags();
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act
        var result = featureManager.IsFeatureEnabled("NonExistentFeature");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Constructor_WhenOptionsIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new FeatureManager(null!));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsFeatureEnabled_DetailedLogging_ReturnsCorrectState(bool expectedState)
    {
        // Arrange
        var featureFlags = new FeatureFlags
        {
            EnableDetailedLogging = expectedState
        };
        var options = Options.Create(featureFlags);
        var featureManager = new FeatureManager(options);

        // Act
        var result = featureManager.IsFeatureEnabled(nameof(FeatureFlags.EnableDetailedLogging));

        // Assert
        Assert.Equal(expectedState, result);
    }
}
