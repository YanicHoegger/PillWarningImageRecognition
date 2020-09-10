#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Clients/WebInterface/WebInterface.csproj", "Clients/WebInterface/"]
COPY ["Clients/Clients.Shared/Clients.Shared.csproj", "Clients/Clients.Shared/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]
COPY ["Bootstrapper/Bootstrapper.csproj", "Bootstrapper/"]
COPY ["BackEnd/DatabaseInteraction/DatabaseInteraction.csproj", "BackEnd/DatabaseInteraction/"]
COPY ["Bootstrapper.Interface/Bootstrapper.Interface.csproj", "Bootstrapper.Interface/"]
COPY ["BackEnd/DatabaseInteraction.Interface/DatabaseInteraction.Interface.csproj", "BackEnd/DatabaseInteraction.Interface/"]
COPY ["BackEnd/Domain/Domain.csproj", "BackEnd/Domain/"]
COPY ["BackEnd/DrugCheckingCrawler.Interface/DrugCheckingCrawler.Interface.csproj", "BackEnd/DrugCheckingCrawler.Interface/"]
COPY ["BackEnd/ImageInteraction.Interface/ImageInteraction.Interface.csproj", "BackEnd/ImageInteraction.Interface/"]
COPY ["BackEnd/Domain.Interface/Domain.Interface.csproj", "BackEnd/Domain.Interface/"]
COPY ["BackEnd/DrugCheckingCrawler/DrugCheckingCrawler.csproj", "BackEnd/DrugCheckingCrawler/"]
COPY ["BackEnd/ImageInteraction/ImageInteraction.csproj", "BackEnd/ImageInteraction/"]
RUN dotnet restore "Clients/WebInterface/WebInterface.csproj"
COPY . .
WORKDIR "/src/Clients/WebInterface"
RUN dotnet build "WebInterface.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebInterface.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebInterface.dll"]