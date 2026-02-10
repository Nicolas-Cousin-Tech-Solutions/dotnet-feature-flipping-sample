---
title: Feature Flipping en .NET 10
theme: black
highlightTheme: monokai
revealOptions:
  transition: 'slide'
  controls: true
  progress: true
  center: true
---

# Feature Flipping

## Maîtriser le déploiement continu avec .NET 10

---

## Qu'est-ce que le Feature Flipping?

Le **Feature Flipping** (ou Feature Toggle) est une technique de développement qui permet d'**activer ou désactiver des fonctionnalités** sans modifier le code source ni redéployer l'application.

---

## Pourquoi utiliser le Feature Flipping?

* 🚀 **Déploiement continu** - Déployer du code non terminé
* 🎯 **Tests A/B** - Tester différentes versions d'une fonctionnalité
* 🛡️ **Gestion des risques** - Désactiver rapidement une fonctionnalité problématique
* 👥 **Rollout progressif** - Activer progressivement pour certains utilisateurs
* 🔧 **Configuration sans redéploiement** - Modifier le comportement en production

---

## Types de Feature Flags

1. **Release Toggles** - Pour le développement progressif
2. **Experiment Toggles** - Pour les tests A/B
3. **Ops Toggles** - Pour la gestion opérationnelle
4. **Permission Toggles** - Pour l'accès basé sur les rôles

---

## Architecture SOLID

Notre implémentation respecte les principes SOLID :

* **S**ingle Responsibility - Chaque classe a une responsabilité unique
* **O**pen/Closed - Extensible sans modification
* **L**iskov Substitution - Interfaces respectées
* **I**nterface Segregation - Interfaces spécifiques
* **D**ependency Inversion - Dépendances sur abstractions

---

## Modèle de configuration

```csharp
namespace FeatureFlippingApi.Models;

public class FeatureFlags
{
    public const string SectionName = "FeatureFlags";
    
    public bool EnableWeatherForecast { get; set; }
    public bool EnableProductCatalog { get; set; }
    public bool EnableDetailedLogging { get; set; }
}
```

Simple et type-safe ✅

---

## Interface IFeatureManager

```csharp
namespace FeatureFlippingApi.Services;

/// <summary>
/// Interface Segregation Principle
/// </summary>
public interface IFeatureManager
{
    /// <summary>
    /// Vérifie si une fonctionnalité est activée
    /// </summary>
    bool IsFeatureEnabled(string featureName);
}
```

Abstraction claire pour la gestion des features 🎯

---

## Implémentation FeatureManager

```csharp
public class FeatureManager : IFeatureManager
{
    private readonly FeatureFlags _featureFlags;

    public FeatureManager(IOptions<FeatureFlags> featureFlags)
    {
        ArgumentNullException.ThrowIfNull(featureFlags);
        _featureFlags = featureFlags.Value ?? 
            throw new ArgumentNullException(nameof(featureFlags));
    }

    public bool IsFeatureEnabled(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
            throw new ArgumentException(
                "Feature name cannot be null or empty");

        var property = typeof(FeatureFlags)
            .GetProperty(featureName);
        
        if (property == null || 
            property.PropertyType != typeof(bool))
            return false;

        return (bool)(property.GetValue(_featureFlags) ?? false);
    }
}
```

---

## Configuration dans appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FeatureFlags": {
    "EnableWeatherForecast": true,
    "EnableProductCatalog": true,
    "EnableDetailedLogging": false
  }
}
```

Configuration centralisée et lisible 📋

---

## Configuration par environnement

**appsettings.Development.json**

```json
{
  "FeatureFlags": {
    "EnableWeatherForecast": true,
    "EnableProductCatalog": false,
    "EnableDetailedLogging": true
  }
}
```

Différentes configurations pour chaque environnement 🌍

---

## Enregistrement des services

```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuration des feature flags
builder.Services.Configure<FeatureFlags>(
    builder.Configuration.GetSection(
        FeatureFlags.SectionName));

// Dependency Injection
builder.Services.AddSingleton<IFeatureManager, 
                               FeatureManager>();
```

Injection de dépendances selon SOLID 💉

---

## Utilisation dans un Endpoint

```csharp
private static Results<Ok<WeatherForecast[]>, 
                       StatusCodeHttpResult> 
    GetWeatherForecast(
        IFeatureManager featureManager,
        ILogger<IFeatureManager> logger)
{
    if (!featureManager.IsFeatureEnabled(
        nameof(FeatureFlags.EnableWeatherForecast)))
    {
        logger.LogWarning(
            "Weather forecast feature is disabled");
        return TypedResults.StatusCode(503);
    }

    var forecast = Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return TypedResults.Ok(forecast);
}
```

---

## Tests unitaires - Setup

```csharp
[Fact]
public void IsFeatureEnabled_WhenEnabled_ReturnsTrue()
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
    var result = featureManager.IsFeatureEnabled(
        nameof(FeatureFlags.EnableWeatherForecast));

    // Assert
    Assert.True(result);
}
```

Test-Driven Development (TDD) 🧪

---

## Tests unitaires - Validation

```csharp
[Fact]
public void IsFeatureEnabled_WhenDisabled_ReturnsFalse()
{
    // Arrange
    var featureFlags = new FeatureFlags
    {
        EnableProductCatalog = false
    };
    var options = Options.Create(featureFlags);
    var featureManager = new FeatureManager(options);

    // Act
    var result = featureManager.IsFeatureEnabled(
        nameof(FeatureFlags.EnableProductCatalog));

    // Assert
    Assert.False(result);
}
```

Validation complète du comportement ✅

---

## Tests unitaires - Edge Cases

```csharp
[Fact]
public void IsFeatureEnabled_WhenNull_ThrowsException()
{
    // Arrange
    var featureFlags = new FeatureFlags();
    var options = Options.Create(featureFlags);
    var featureManager = new FeatureManager(options);

    // Act & Assert
    Assert.Throws<ArgumentException>(
        () => featureManager.IsFeatureEnabled(null!));
}
```

Gestion robuste des cas limites 🛡️

---

## Exemple d'API complète

```csharp
app.MapWeatherEndpoints();
app.MapProductEndpoints();

app.MapGet("/api/health", 
    () => Results.Ok(new { 
        status = "healthy", 
        timestamp = DateTime.UtcNow 
    }))
    .WithName("HealthCheck");
```

Endpoints modulaires et testables 🎯

---

## Démonstration

### Avec la feature activée (Production)

```bash
$ curl http://localhost:5078/api/weather
[
  {
    "date": "2026-02-11",
    "temperatureC": 27,
    "summary": "Balmy",
    "temperatureF": 80
  },
  ...
]
```

✅ **Status: 200 OK**

---

## Démonstration

### Avec la feature désactivée (Development)

```bash
$ curl -i http://localhost:5078/api/products
HTTP/1.1 503 Service Unavailable
```

❌ **Status: 503 Service Unavailable**

La fonctionnalité est désactivée via configuration!

---

## Avantages du Feature Flipping

* ✅ **DRY** - Don't Repeat Yourself - Configuration centralisée
* ✅ **KISS** - Keep It Simple, Stupid - Solution simple et efficace
* ✅ **SOLID** - Principes de conception respectés
* ✅ **TDD** - Tests unitaires complets
* ✅ **Flexibilité** - Changement de comportement sans redéploiement

---

## Bonnes pratiques

1. **Limiter la durée de vie** - Supprimer les flags après déploiement complet
2. **Nommer clairement** - Noms explicites et cohérents
3. **Documenter** - Expliquer l'objectif de chaque flag
4. **Tester** - Tester tous les états possibles
5. **Monitorer** - Logger l'utilisation des flags

---

## Évolutions possibles

* 🔄 **Feature flags dynamiques** - Modification sans redémarrage
* 📊 **Dashboard de gestion** - Interface de contrôle
* 👥 **Feature flags par utilisateur** - Ciblage spécifique
* 📈 **Métriques et analytics** - Suivi de l'utilisation
* 🔌 **Intégration avec Azure App Configuration**

---

## Structure du projet

```
FeatureFlippingApi/
├── Features/
│   ├── WeatherForecast/
│   │   └── WeatherEndpoints.cs
│   └── Products/
│       └── ProductEndpoints.cs
├── Models/
│   ├── FeatureFlags.cs
│   ├── WeatherForecast.cs
│   └── Product.cs
├── Services/
│   ├── IFeatureManager.cs
│   └── FeatureManager.cs
└── Program.cs
```

Organisation claire et modulaire 📁

---

## Code source complet

Le code source complet est disponible sur GitHub :

**Nicolas-Cousin-Tech-Solutions/dotnet-feature-flipping-sample**

* ✅ .NET 10 / C# 14
* ✅ SOLID, DRY, KISS, TDD
* ✅ Tests unitaires complets
* ✅ Documentation complète

---

## Résumé

Le **Feature Flipping** est une technique puissante pour :

* Déployer en continu avec confiance
* Tester en production de manière contrôlée
* Réduire les risques de déploiement
* Améliorer la flexibilité opérationnelle

**Adoptez-le dans vos projets .NET 10!** 🚀

---

## Questions?

Merci pour votre attention! 👏

---

## Ressources

* [Martin Fowler - Feature Toggles](https://martinfowler.com/articles/feature-toggles.html)
* [Microsoft Learn - Feature Management](https://learn.microsoft.com/aspnet/core/fundamentals/feature-flags)
* [.NET 10 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10)
* [SOLID Principles](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/architectural-principles)

---

# Merci! 🎉

**Contact:** nicolas@tech-solutions.example

**GitHub:** Nicolas-Cousin-Tech-Solutions
