#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
CMD ["dotnet", "dev-certs", "https", "&&", "dotnet", "run"]
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Game.Gateway.Api/Game.Gateway.Api.csproj", "Game.Gateway.Api/"]
RUN dotnet restore "Game.Gateway.Api/Game.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/Game.Gateway.Api"
RUN dotnet build "Game.Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Game.Gateway.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game.Gateway.Api.dll"]