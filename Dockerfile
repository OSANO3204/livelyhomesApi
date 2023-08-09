# Base stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 587

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG CACHEBUST=1
WORKDIR /src
COPY ["HousingProject.API/HousingProject.API.csproj", "HousingProject.API/"]
COPY ["HousingProject.Architecture/HousingProject.Infrastructure.csproj", "HousingProject.Architecture/"]
COPY ["HousingProject.Core/HousingProject.Core.csproj", "HousingProject.Core/"]
RUN dotnet restore "HousingProject.API/HousingProject.API.csproj"
COPY . .
WORKDIR "/src/HousingProject.API"
RUN dotnet build "HousingProject.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "HousingProject.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy HTML templates


ENTRYPOINT ["dotnet", "HousingProject.API.dll"]
