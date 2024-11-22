FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

RUN dotnet restore "src/ProjectName.Persistence/ProjectName.Persistence.csproj"
WORKDIR "/src/src/ProjectName.Persistence"
RUN dotnet build "ProjectName.Persistence.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectName.Persistence.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectName.Persistence.dll"]
