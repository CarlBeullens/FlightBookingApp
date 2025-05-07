FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["FlightService/FlightService.csproj", "FlightService/"]
COPY ["SharedService/SharedService.csproj", "SharedService/"]

# Copy central package management files
COPY ["Directory.Packages.props", "./"]

# Restore the project
RUN dotnet restore "FlightService/FlightService.csproj"

# Copy everything else
COPY . .

WORKDIR "/src/FlightService"
RUN dotnet build "FlightService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FlightService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightService.dll"]