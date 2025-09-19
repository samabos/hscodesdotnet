# HS Codes Classification System

A comprehensive .NET application for Harmonized System (HS) code classification and commodity search, featuring AI-powered prediction capabilities and a modern web interface.

## Overview

This system provides intelligent HS code classification for international trade, combining traditional database search with machine learning predictions to help users find the most appropriate HS codes for their products.

## Features

- **AI-Powered Classification**: Machine learning model for automatic HS code prediction
- **Hierarchical Navigation**: Browse HS codes through structured categories
- **Product Search**: Search and classify products with detailed descriptions
- **Customs Tariff Information**: Access tariff rates and import/export requirements
- **Currency Exchange Rates**: Real-time currency conversion support
- **Document Management**: Store and manage classification documents
- **Search Analytics**: Track and log search patterns for optimization
- **Multi-Environment Support**: Development, staging, and production configurations

## Architecture

The application follows a clean architecture pattern with the following layers:

- **Autumn.UI**: Web application with Razor Pages
- **Autumn.API**: RESTful API with Swagger documentation
- **Autumn.BL**: Business logic and services
- **Autumn.Domain**: Domain models and entities
- **Autumn.Repository**: Data access layer
- **Autumn.UIML.Model**: Machine learning model for predictions

## Technology Stack

- **.NET 9.0**: Core framework
- **ASP.NET Core**: Web framework
- **MongoDB**: Document database for HS codes and products
- **SQL Server**: Relational database for structured data
- **ML.NET**: Machine learning framework
- **AutoMapper**: Object mapping
- **Auth0**: Authentication and authorization
- **Docker**: Containerization
- **Nginx**: Reverse proxy and load balancer

## Prerequisites

- .NET 9.0 SDK
- MongoDB
- SQL Server
- Docker (optional)

## Quick Start

### Using Docker (Recommended)

1. Clone the repository:

```bash
git clone <repository-url>
cd hscodesdotnet
```

2. Start the application:

```bash
docker-compose up -d
```

The application will be available at `http://localhost`

### Manual Setup

1. **Configure Databases**:

   - Set up MongoDB instance
   - Configure SQL Server connection
   - Update connection strings in `appsettings.json`

2. **Build and Run**:

```bash
dotnet restore
dotnet build
dotnet run --project Autumn.UI
```

3. **API Documentation**:
   - Navigate to `/swagger` for API documentation
   - Available at `http://localhost:5000/swagger`

## Configuration

### Database Settings

Update the following in `appsettings.json`:

```json
{
  "StoreDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ClassificationDb"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  }
}
```

### Authentication

Configure Auth0 settings:

```json
{
  "Auth0": {
    "Domain": "your-auth0-domain",
    "ClientId": "your-client-id"
  }
}
```

## API Endpoints

### Search Operations

- `GET /api/v1/search` - Search HS codes
- `GET /api/v1/classify/commodity` - Classify commodity
- `GET /api/v1/note` - Get classification notes

### Data Management

- `GET /api/v1/hscode` - Retrieve HS codes
- `GET /api/v1/product` - Product information
- `GET /api/v1/currency` - Currency exchange rates

## Machine Learning

The system includes a pre-trained ML model for HS code prediction:

- **Model**: ML.NET classification model
- **Input**: Product descriptions and keywords
- **Output**: Predicted HS codes with confidence scores
- **Threshold**: Configurable confidence threshold (default: 0.02)

## Development

### Project Structure

```
├── Autumn.UI/              # Web application
├── Autumn.API/             # REST API
├── Autumn.BL/              # Business logic
├── Autumn.Domain/          # Domain models
├── Autumn.Repository/      # Data access
├── Autumn.UIML.Model/      # ML model
└── docker-compose.yml      # Container orchestration
```

### Adding New Features

1. Create domain models in `Autumn.Domain`
2. Implement repository interfaces in `Autumn.Repository`
3. Add business logic in `Autumn.BL`
4. Create API controllers in `Autumn.API`
5. Update UI pages in `Autumn.UI`

## Deployment

### Production Deployment

1. **Build Docker Image**:

```bash
docker build -t hscodes:latest .
```

2. **Deploy with Docker Compose**:

```bash
docker-compose -f docker-compose.prod.yml up -d
```

3. **Configure Nginx**:
   - Update `nginx.conf` for your domain
   - Set up SSL certificates
   - Configure load balancing

### Environment Variables

Set the following environment variables for production:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__DefaultConnection`
- `StoreDatabaseSettings__ConnectionString`
- `Auth0__Domain`
- `Auth0__ClientId`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support and questions:

- Create an issue in the repository
- Contact the development team
- Check the API documentation at `/swagger`

## Changelog

### Version 1.09

- Updated to .NET 9.0
- Enhanced ML model accuracy
- Improved search performance
- Added Docker support
- Updated authentication system
