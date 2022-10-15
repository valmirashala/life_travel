FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /
COPY ["Travelephant/nuget.config", "./"]
COPY ["Travelephant/Travelephant.csproj", "Travelephant/"]

RUN dotnet restore "Travelephant/Travelephant.csproj" --configfile ./nuget.config
COPY . .

WORKDIR "/Travelephant"
RUN dotnet build "Travelephant.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Travelephant.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Travelephant.dll"]
