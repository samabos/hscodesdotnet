# Stage 1: Build React SPA
FROM node:20-alpine AS spa-build
ARG VITE_AUTH0_DOMAIN=""
ARG VITE_AUTH0_CLIENT_ID=""
ARG VITE_AUTH0_AUDIENCE=""
WORKDIR /app
COPY Autumn.SPA/package*.json ./
RUN npm ci
COPY Autumn.SPA/ ./
RUN npm run build

# Stage 2: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS api-build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Autumn.API/Autumn.API.csproj", "Autumn.API/"]
COPY ["Autumn.BL/Autumn.Service.csproj", "Autumn.BL/"]
COPY ["Autumn.Domain/Autumn.Domain.csproj", "Autumn.Domain/"]
COPY ["Autumn.Repository/Autumn.Infrastructure.csproj", "Autumn.Repository/"]
COPY ["Autumn.UIML.Model/Autumn.UIML.Model.csproj", "Autumn.UIML.Model/"]
RUN dotnet restore "Autumn.API/Autumn.API.csproj"
COPY . .
WORKDIR "/src/Autumn.API"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=api-build /app/publish .
COPY --from=spa-build /app/dist ./wwwroot
EXPOSE 8080
ENTRYPOINT ["dotnet", "Autumn.API.dll"]
