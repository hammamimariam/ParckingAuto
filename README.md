# ParckingAuto

**Système de gestion de parc automobile** pour le contexte tunisien.

Application web complète de gestion d'une flotte de véhicules : suivi des véhicules, traçabilité des entrées/sorties du parking, gestion du carburant et de la maintenance, archivage des documents administratifs et génération d'alertes automatiques.

---

## Fonctionnalités

| Module | Description |
|--------|-------------|
| **Véhicules** | Fiche complète (immatriculation `XXX TUN XXXX`, carte grise tunisienne, assurance, visite technique), CRUD, suivi km |
| **Mouvements (parking)** | Enregistrement sortie/retour avec date/heure et km, mise à jour automatique du kilométrage |
| **Carburant** | Saisie des pleins, calcul de la consommation moyenne (L/100 km) |
| **Maintenance** | Historique des interventions, coûts, factures jointes |
| **Alertes** | Vidange (9 000 / 10 000 km), assurance (J-30), visite technique, permis chauffeur |
| **Statistiques** | Tableaux de bord, coûts mensuels, graphiques |
| **Documents** | Archivage carte grise, assurance, factures, bons de réparation |
| **Sécurité** | Authentification JWT, 3 rôles avec droits différenciés |

### Acteurs et rôles

| Rôle | Accès principal |
|------|-----------------|
| **Administrateur** | Dashboard, Settings, Utilisateurs, tout le reste |
| **Gestionnaire** | Véhicules, chauffeurs, mouvements, carburant, maintenance, alertes, stats |
| **Chauffeur** | Enregistrement sortie/retour (Mes missions) |

---

## Stack technique

| Composant | Technologie |
|-----------|-------------|
| Backend | ASP.NET Core 9, Entity Framework Core 9, AutoMapper |
| Frontend | Blazor WebAssembly, Bootstrap 5, Chart.js |
| Base de données | MySQL 8 (fournisseur Pomelo) |
| Authentification | JWT Bearer + BCrypt |
| Docs API | Swagger (développement) |
| PDF | QuestPDF |

### Architecture

```
Navigateur (Blazor WebAssembly - Front/)
        │  REST JSON / HTTPS
        ▼
API ASP.NET Core 9 (ParckingAuto/)
Controllers → Services → Repositories → ParcAutoContext
        │  (AutoMapper DTO ↔ Entités)
        ▼
MySQL 8 (parc_auto)

Partagee/  →  DTO partagés entre l'API et le Front
```

### Structure de la solution

| Projet | Rôle |
|--------|------|
| `ParckingAuto` | API REST, modèles EF, services métier, migrations |
| `Front` | Interface Blazor WebAssembly |
| `Partagee` | DTO communs (`VehiculeDto`, `MouvementDto`, …) |

---

## Prérequis

- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download)
- MySQL 8 (serveur local `localhost` / `3306`)
- Un IDE (Visual Studio 2022+, VS Code, Rider)

---

## Installation et démarrage

### 1. Configurer la base de données

La chaîne de connexion par défaut se trouve dans `ParckingAuto/appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=parc_auto;user=root;password=;"
}
```

Adaptez `user` / `password` à votre installation MySQL.

### 2. Lancer l'API

```bash
cd ParckingAuto
dotnet run
```

La base `parc_auto` est créée et migrée automatiquement au démarrage (`Database.Migrate()` + `DbInitializer.Seed()`).

- URL HTTP : http://localhost:5199
- URL HTTPS : https://localhost:7275

### 3. Lancer le Front (Blazor WASM)

Dans un second terminal :

```bash
cd Front
dotnet run
```

- URL HTTP : http://localhost:5228
- URL HTTPS : https://localhost:7042

### Compte par défaut

| Email | Mot de passe | Rôle |
|-------|--------------|------|
| `admin@parc.com` | `Parc@0` | Administrateur |

---

## Principaux endpoints API

| Méthode | Route | Rôle |
|---------|-------|------|
| POST | `/api/Auth/login` | Public |
| GET | `/api/Vehicules` | Tous (authentifié) |
| GET | `/api/Vehicules/{id}/suivi` | Tous (authentifié) |
| POST / PUT | `/api/Vehicules` | Admin, Gestionnaire |
| DELETE | `/api/Vehicules/{id}` | Admin |
| POST / PUT | `/api/Mouvements` | Tous |
| GET / POST | `/api/Carburant` | Admin, Gestionnaire |
| GET / POST | `/api/Maintenance` | Admin, Gestionnaire |
| GET / PUT | `/api/Alertes` | Admin, Gestionnaire |
| PUT | `/api/Settings/update` | Admin |

---

## Pages de l'interface

| Route | Page | Rôles |
|-------|------|-------|
| `/login` | Connexion | Public |
| `/dashboard` | Tableau de bord | Admin, Gestionnaire |
| `/vehicules` | Gestion des véhicules | Admin, Gestionnaire |
| `/vehicule/{id}` | Suivi véhicule | Admin, Gestionnaire |
| `/chauffeurs` | Gestion des chauffeurs | Admin, Gestionnaire |
| `/mouvements` | Mouvements parking | Tous |
| `/carburant` | Carburant | Admin, Gestionnaire |
| `/maintenance` | Maintenance | Admin, Gestionnaire |
| `/alertes` | Alertes | Admin, Gestionnaire |
| `/statistiques` | Statistiques | Admin, Gestionnaire |
| `/settings` | Paramètres | Admin |
| `/utilisateurs` | Collaborateurs | Admin |

---

## Règles d'alertes

- **Vidange** : pré-alerte à 9 000 km, critique à 10 000 km depuis la dernière vidange
- **Assurance** : J-30 / J-15 / J-7 avant expiration
- **Visite technique** : idem assurance
- **Permis chauffeur** : expiration du permis dans les 30 jours

---

## Sécurité

- Authentification JWT Bearer avec expiration configurable
- Mots de passe hachés avec BCrypt
- Autorisation par rôles via `[Authorize(Roles = "...")]`
- CORS autorisant les origines du Front en localhost

---

## Règles de contribution (Git)

Le dépôt contient un `.gitignore` qui exclut les artefacts de build (`bin/`, `obj/`, `.vs/`, `*.user`). Ne committez jamais ces dossiers ni les fichiers de configuration contenant des secrets.

---

*Documentation générée pour le projet ParckingAuto — [hammamimariam/ParckingAuto](https://github.com/hammamimariam/ParckingAuto)*
