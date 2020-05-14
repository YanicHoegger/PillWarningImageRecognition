#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebInterface/Server/WebInterface.Server.csproj", "WebInterface/Server/"]
COPY ["WebInterface/Client/WebInterface.Client.csproj", "WebInterface/Client/"]
COPY ["WebInterface/Shared/WebInterface.Shared.csproj", "WebInterface/Shared/"]
RUN dotnet restore "WebInterface/Server/WebInterface.Server.csproj"
COPY . .
WORKDIR "/src/WebInterface/Server"
RUN dotnet build "WebInterface.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebInterface.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebInterface.Server.dll"]