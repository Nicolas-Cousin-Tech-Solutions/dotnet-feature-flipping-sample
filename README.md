# dotnet-feature-flipping-sample

Exemple d'implémentation de Feature Flipping (Feature Toggle) en .NET 10 avec C# 14, suivant les principes TDD, DRY, KISS et SOLID.

## 🎯 Objectif

Ce projet démontre comment implémenter et utiliser le **Feature Flipping** dans une API .NET 10 pour activer ou désactiver des fonctionnalités par configuration, sans modification de code ni redéploiement.

## 🏗️ Architecture

Le projet est structuré selon les principes SOLID :

- **Single Responsibility** : Chaque classe a une responsabilité unique
- **Open/Closed** : Le système est extensible sans modification
- **Liskov Substitution** : Les interfaces sont respectées
- **Interface Segregation** : Interfaces spécifiques et ciblées
- **Dependency Inversion** : Dépendances sur des abstractions

## 📁 Structure du projet

```
FeatureFlippingApi/
├── Features/
│   ├── WeatherForecast/
│   │   └── WeatherEndpoints.cs      # Endpoints météo avec feature flag
│   └── Products/
│       └── ProductEndpoints.cs       # Endpoints produits avec feature flag
├── Models/
│   ├── FeatureFlags.cs              # Configuration des feature flags
│   ├── WeatherForecast.cs           # Modèle de données météo
│   └── Product.cs                   # Modèle de données produit
├── Services/
│   ├── IFeatureManager.cs           # Interface du gestionnaire de features
│   └── FeatureManager.cs            # Implémentation du gestionnaire
└── Program.cs                       # Point d'entrée et configuration

FeatureFlippingApi.Tests/
└── Services/
    └── FeatureManagerTests.cs       # Tests unitaires complets
```

## 🚀 Démarrage rapide

### Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Installation

```bash
# Cloner le repository
git clone https://github.com/Nicolas-Cousin-Tech-Solutions/dotnet-feature-flipping-sample.git
cd dotnet-feature-flipping-sample

# Restaurer les dépendances
dotnet restore

# Compiler le projet
dotnet build
```

### Exécution

```bash
# Lancer l'API
cd FeatureFlippingApi
dotnet run

# L'API sera disponible sur http://localhost:5078
```

### Tests

```bash
# Exécuter tous les tests
dotnet test

# Exécuter les tests avec couverture
dotnet test /p:CollectCoverage=true
```

## 📡 Endpoints disponibles

### Health Check (toujours actif)
```bash
GET /api/health
```

### Weather Forecast (contrôlé par feature flag)
```bash
GET /api/weather
```

### Products (contrôlé par feature flag)
```bash
GET /api/products
GET /api/products/{id}
```

## ⚙️ Configuration des Feature Flags

Les feature flags sont configurés dans `appsettings.json` :

```json
{
  "FeatureFlags": {
    "EnableWeatherForecast": true,
    "EnableProductCatalog": true,
    "EnableDetailedLogging": false
  }
}
```

### Configuration par environnement

Créez des fichiers `appsettings.{Environment}.json` pour différentes configurations :

**appsettings.Development.json** :
```json
{
  "FeatureFlags": {
    "EnableWeatherForecast": true,
    "EnableProductCatalog": false,
    "EnableDetailedLogging": true
  }
}
```

**appsettings.Production.json** :
```json
{
  "FeatureFlags": {
    "EnableWeatherForecast": true,
    "EnableProductCatalog": true,
    "EnableDetailedLogging": false
  }
}
```

## 💡 Exemples d'utilisation

### Tester une feature activée

```bash
curl http://localhost:5078/api/weather
# Retourne les données météo (200 OK)
```

### Tester une feature désactivée

```bash
curl -i http://localhost:5078/api/products
# Retourne 503 Service Unavailable
```

## 🧪 Tests

Le projet inclut des tests unitaires complets suivant le principe TDD :

- ✅ Tests de la logique du FeatureManager
- ✅ Tests des cas nominaux
- ✅ Tests des cas d'erreur
- ✅ Tests des cas limites (null, empty, inexistant)

Exécutez `dotnet test` pour vérifier que tout fonctionne correctement.

## 📊 Présentation reveal.js

Une présentation complète au format reveal.js est disponible dans le répertoire `/docs/slides.md`.

Pour visualiser la présentation :

1. Installez [reveal-md](https://github.com/webpro/reveal-md) :
   ```bash
   npm install -g reveal-md
   ```

2. Lancez la présentation :
   ```bash
   reveal-md docs/slides.md
   ```

3. Ouvrez votre navigateur sur http://localhost:1948

## 🎓 Concepts clés

### Qu'est-ce que le Feature Flipping ?

Le Feature Flipping (ou Feature Toggle) permet de :
- 🚀 Déployer du code non terminé en production
- 🎯 Effectuer des tests A/B
- 🛡️ Désactiver rapidement une fonctionnalité problématique
- 👥 Activer progressivement des fonctionnalités pour certains utilisateurs
- 🔧 Modifier le comportement sans redéploiement

### Principes appliqués

- **DRY** : Configuration centralisée, pas de duplication
- **KISS** : Solution simple et efficace
- **SOLID** : Architecture propre et maintenable
- **TDD** : Tests avant implémentation

## 📚 Documentation

- [Slides de présentation](docs/slides.md) - Présentation complète en reveal.js
- [API Documentation](FeatureFlippingApi/FeatureFlippingApi.http) - Exemples de requêtes HTTP

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à :
- Ouvrir des issues pour signaler des bugs
- Proposer des améliorations
- Soumettre des pull requests

## 📝 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## 🔗 Ressources

- [Martin Fowler - Feature Toggles](https://martinfowler.com/articles/feature-toggles.html)
- [Microsoft Learn - Feature Management](https://learn.microsoft.com/aspnet/core/fundamentals/feature-flags)
- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [SOLID Principles](https://learn.microsoft.com/dotnet/architecture/modern-web-apps-azure/architectural-principles)

## 👨‍💻 Auteur

Nicolas Cousin - Tech Solutions

---

⭐ Si ce projet vous a été utile, n'hésitez pas à lui donner une étoile !
