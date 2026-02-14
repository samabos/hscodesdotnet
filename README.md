# HS Codes Classification System

A comprehensive .NET application for Harmonized System (HS) code classification and commodity search, featuring AI-powered prediction, LLM-assisted classification, and a modern React SPA.

## Overview

This system provides intelligent HS code classification for international trade, combining MongoDB Atlas Search, machine learning predictions, and LLM-powered classification (Groq) to help users find the most appropriate HS codes for their products.

## Features

- **Blended Search Pipeline**: Multi-stage search combining exact match, Atlas Search with fuzzy matching, description search, and LLM fallback
- **LLM Classification (Groq)**: Automatic fallback to Llama 3.1 for natural language product queries when database results are low-confidence
- **ML.NET Prediction**: Pre-trained classification model for HS code prediction
- **MongoDB Atlas Search**: Full-text search with fuzzy matching on products and HS code descriptions
- **Modern React SPA**: Tailwind CSS frontend with dark mode, autocomplete, and hierarchy navigation
- **Duty Calculator**: Import duty and tax calculation with multi-country support
- **Hierarchical Navigation**: Browse HS codes through structured categories (Section > Chapter > Heading)
- **Rate Limiting**: Per-IP request throttling to protect the API
- **Search Analytics**: Groq prediction logging with accuracy tracking
- **Auth0 Authentication**: Secured admin endpoints with JWT
- **Currency Exchange Rates**: Real-time currency conversion support

## Architecture

The application follows a clean architecture pattern:

```
├── Autumn.SPA/              # React SPA (Vite + Tailwind CSS)
├── Autumn.API/              # Minimal API endpoints (.NET 10)
├── Autumn.BL/               # Business logic & services
├── Autumn.Domain/           # Domain models & entities
├── Autumn.Repository/       # MongoDB & SQL data access
├── Autumn.UIML.Model/       # ML.NET prediction model
└── docker-compose.yml       # Container orchestration
```

## Technology Stack

- **.NET 10.0**: Core framework
- **ASP.NET Core Minimal APIs**: RESTful endpoints with Swagger
- **React 19 + Vite**: Single-page application
- **Tailwind CSS 4**: Utility-first styling with light/dark theme
- **MongoDB Atlas**: Document database with Atlas Search
- **SQL Server**: Relational database for structured data
- **ML.NET 1.5**: Machine learning classification
- **Groq API (Llama 3.1 8B)**: LLM-powered HS code classification fallback
- **Auth0**: JWT authentication and authorization
- **Docker + Nginx**: Containerization and reverse proxy

## Prerequisites

- .NET 10.0 SDK
- Node.js 18+
- MongoDB (Atlas recommended for full-text search)
- Docker (optional)

## Quick Start

### Using Docker (Recommended)

```bash
git clone https://github.com/samabos/hscodesdotnet.git
cd hscodesdotnet
docker-compose up -d
```

The application will be available at `http://localhost`

### Manual Setup

1. **Configure Settings** — Update `Autumn.API/appsettings.json`:

```json
{
  "StoreDatabaseSettings": {
    "ConnectionString": "mongodb+srv://...",
    "DatabaseName": "ClassificationDb"
  },
  "SiteSettings": {
    "Threshold": "0.1",
    "GroqApiKey": "your-groq-api-key",
    "GroqModel": "llama-3.1-8b-instant"
  },
  "Auth0": {
    "Domain": "https://your-auth0-domain.auth0.com/",
    "Audience": "autumnapi"
  }
}
```

2. **Run the API**:

```bash
dotnet run --project Autumn.API
```

3. **Run the SPA** (development):

```bash
cd Autumn.SPA
npm install
npm run dev
```

4. **API Documentation**: Navigate to `/swagger`

## Search Pipeline

The search system uses a blended multi-stage approach:

| Stage | Source | Confidence | Description |
|-------|--------|-----------|-------------|
| 1 | Exact Match | 0.88–0.97 | Direct keyword match in products collection |
| 2 | Atlas Search | 0.60–0.82 | Fuzzy full-text search with word splitting |
| 3 | Description Search | 0.40–0.73 | Atlas Search on HS code descriptions (Level 3–4) |
| 4 | Groq LLM | 0.45–0.75 | Llama 3.1 classification (fallback when best result < 0.70) |
| 5 | Synonyms | 0.35–0.58 | RapidAPI synonym expansion (fallback) |
| 6 | ML.NET Model | Variable | Pre-trained classifier (last resort) |

Stages 1–3 run concurrently. Stage 4 only triggers when no high-confidence results are found. Results are deduplicated by HS code, keeping the highest-confidence entry.

## API Endpoints

### Public Endpoints (rate limited: 30 req/min per IP)

- `GET /api/search` — Search and classify products
- `GET /api/browse` — Browse HS code hierarchy
- `GET /api/duty` — Calculate import duties and taxes
- `GET /api/note/{hscode}` — Get notes, documents, and tariffs
- `GET /api/codelist/countries` — List countries
- `GET /api/codelist/currency` — Currency exchange rates
- `GET /api/codelist/products/{query?}` — Product autocomplete

### Admin Endpoints (requires Auth0 JWT)

- `GET /api/admin/dashboard` — Dashboard statistics
- CRUD: `/api/admin/products`, `/api/admin/codes`, `/api/admin/tariffs`
- `GET /api/admin/querylogs` — Search analytics

## MongoDB Atlas Search Indexes

Create these indexes on your Atlas cluster for optimal search:

**`product-index`** on `products` collection:
```json
{ "mappings": { "dynamic": true, "fields": { "Keyword": { "type": "string", "analyzer": "lucene.standard" } } } }
```

**`hscodes-index`** on `hscodes` collection:
```json
{ "mappings": { "dynamic": true, "fields": { "Description": { "type": "string", "analyzer": "lucene.standard" }, "Level": { "type": "number" } } } }
```

## Groq LLM Integration

The system uses Groq's free API tier (Llama 3.1 8B) as a smart fallback:

- Only called when database stages return low-confidence results (< 0.70)
- Predictions are logged to `SearchLog` with `Source = "groq"` and `FoundInDb` flag for accuracy tracking
- Free tier: 14,400 requests/day, 30 requests/minute
- Get your API key at [console.groq.com](https://console.groq.com)

## Deployment

### Production with Docker

```bash
docker build -t hscodes:latest .
docker-compose -f docker-compose.prod.yml up -d
```

### Environment Variables

- `ASPNETCORE_ENVIRONMENT=Production`
- `StoreDatabaseSettings__ConnectionString`
- `SiteSettings__GroqApiKey`
- `Auth0__Domain`
- `Auth0__Audience`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Changelog

### Version 3.0

- Upgraded to .NET 10.0 with Minimal APIs
- New React 19 SPA with Tailwind CSS 4 (replaces Razor Pages)
- Light/dark theme with modern UI
- MongoDB Atlas Search with fuzzy matching
- Groq LLM integration (Llama 3.1 8B) as smart classification fallback
- Blended concurrent search pipeline (exact + Atlas + description + LLM)
- Groq prediction logging with accuracy tracking (`Source`, `FoundInDb` fields)
- Per-IP rate limiting (30 req/min)
- Rate limit toast notifications in frontend
- Product autocomplete with debounced search
- Hierarchy-style browse navigation
- Import duty calculator with multi-country support
- Cached ML.NET model (Lazy singleton)
- Tokenized regex fallback for searches with spaces/hyphens

### Version 1.09

- Updated to .NET 9.0
- Enhanced ML model accuracy
- Improved search performance
- Added Docker support
- Updated authentication system
