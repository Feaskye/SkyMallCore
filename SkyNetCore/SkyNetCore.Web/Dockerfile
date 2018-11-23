FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["SkyNetCore.Web/SkyNetCore.Web.csproj", "SkyNetCore.Web/"]
COPY ["SkyNetCore.Common/SkyNetCore.Common.csproj", "SkyNetCore.Common/"]
RUN dotnet restore "SkyNetCore.Web/SkyNetCore.Web.csproj"
COPY . .
WORKDIR "/src/SkyNetCore.Web"
RUN dotnet build "SkyNetCore.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SkyNetCore.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SkyNetCore.Web.dll"]