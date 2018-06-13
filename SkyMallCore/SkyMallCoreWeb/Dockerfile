FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY SkyMallCoreWeb/SkyMallCoreWeb.csproj SkyMallCoreWeb/
COPY SkyMallCore.Core/SkyMallCore.Core.csproj SkyMallCore.Core/
COPY SkyMallCore.Models/SkyMallCore.Models.csproj SkyMallCore.Models/
COPY SkyMallCore.Services/SkyMallCore.Services.csproj SkyMallCore.Services/
COPY SkyMallCore.Respository/SkyMallCore.Respository.csproj SkyMallCore.Respository/
COPY SkyMallCore.ViewModel/SkyMallCore.ViewModel.csproj SkyMallCore.ViewModel/
COPY SkyMallCore.Data/SkyMallCore.Data.csproj SkyMallCore.Data/
RUN dotnet restore SkyMallCoreWeb/SkyMallCoreWeb.csproj
COPY . .
WORKDIR /src/SkyMallCoreWeb
RUN dotnet build SkyMallCoreWeb.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SkyMallCoreWeb.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SkyMallCoreWeb.dll"]
