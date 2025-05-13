# --------------------------
# Build stage
# --------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["SportifyApi/SportifyApi.csproj", "SportifyApi/"]
RUN dotnet restore "SportifyApi/SportifyApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/SportifyApi
RUN dotnet publish -c Release -o /app/publish

# --------------------------
# Runtime stage
# --------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy from build stage
COPY --from=build /app/publish .

# Expose port for Docker (match Kestrel)
EXPOSE 5000

# Set environment (optional but useful)
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "SportifyApi.dll"]
