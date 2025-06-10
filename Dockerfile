# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install EF CLI globally
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy project file and restore dependencies
COPY ["SportifyApi/SportifyApi.csproj", "SportifyApi/"]
RUN dotnet restore "SportifyApi/SportifyApi.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/SportifyApi
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Set environment and start app
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "SportifyApi.dll"]
