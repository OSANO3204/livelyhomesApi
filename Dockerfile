#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HousingProject.API/HousingProject.API.csproj", "HousingProject.API/"]
COPY ["HousingProject.Architecture/HousingProject.Infrastructure.csproj", "HousingProject.Architecture/"]
COPY ["HousingProject.Core/HousingProject.Core.csproj", "HousingProject.Core/"]
RUN dotnet restore "HousingProject.API/HousingProject.API.csproj"
COPY . .
WORKDIR "/src/HousingProject.API"
RUN dotnet build "HousingProject.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HousingProject.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HousingProject.API.dll"]