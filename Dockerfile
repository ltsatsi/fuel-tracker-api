# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FuelTrackerAPI/FuelTrackerAPI.csproj", "FuelTrackerAPI/"]
COPY ["Domain_Layer/Domain_Layer.csproj", "Domain_Layer/"]
COPY ["Repository_Layer/Repository_Layer.csproj", "Repository_Layer/"]
COPY ["Service_Layer/Service_Layer.csproj", "Service_Layer/"]

RUN dotnet restore "FuelTrackerAPI/FuelTrackerAPI.csproj"

COPY . .
RUN dotnet publish "FuelTrackerAPI/FuelTrackerAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FuelTrackerAPI.dll"]