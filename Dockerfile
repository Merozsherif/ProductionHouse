# -------- Build Stage --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (better layer caching)
COPY ProductionHouse.sln .
COPY ProductionHouse.API/*.csproj ProductionHouse.API/
COPY ProductionHouse.Core/*.csproj ProductionHouse.Core/
COPY ProductionHouse.Infrastructure/*.csproj ProductionHouse.Infrastructure/

RUN dotnet restore ProductionHouse.sln

# Copy everything else and build
COPY . .
WORKDIR /src/ProductionHouse.API
RUN dotnet publish -c Release -o /app/publish

# -------- Runtime Stage --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Railway injects PORT env variable - the app must listen on it
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductionHouse.API.dll"]
