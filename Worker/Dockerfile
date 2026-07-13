FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Worker/CineQueue.Worker.csproj", "Worker/"]
COPY ["Shared/CineQueue.Shared.csproj", "Shared/"]
RUN dotnet restore "Worker/CineQueue.Worker.csproj"

COPY . .
WORKDIR "/src/Worker"
RUN dotnet build "CineQueue.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CineQueue.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false       
# /p:UseAppHost=false -> This option is used to prevent the generation of a platform-specific executable when publishing a .NET application.

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CineQueue.Worker.dll"]