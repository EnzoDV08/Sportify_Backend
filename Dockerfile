FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src
COPY ["SportifyApi/SportifyApi.csproj", "SportifyApi/"]
RUN dotnet restore "SportifyApi/SportifyApi.csproj"

COPY . .
WORKDIR /src/SportifyApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "SportifyApi.dll"]