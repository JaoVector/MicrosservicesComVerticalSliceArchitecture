#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
CMD ["dotnet", "dev-certs", "https", "&&", "dotnet", "run"]
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Game.Inventario.Api/Game.Inventario.Api.csproj", "Game.Inventario.Api/"]
RUN dotnet restore "Game.Inventario.Api/Game.Inventario.Api.csproj"
COPY . .
WORKDIR "/src/Game.Inventario.Api"
RUN dotnet build "Game.Inventario.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.Inventario.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.Inventario.Api.dll"]