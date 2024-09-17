FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5014

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["technosavvy.watcher.csproj", "technosavvy.watcher/"]
RUN dotnet restore "technosavvy.watcher/technosavvy.watcher.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "technosavvy.watcher.csproj" -c Release -o /app/build /p:UseAppHost=false

FROM build AS publish
RUN dotnet publish "technosavvy.watcher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "technosavvy.watcher.dll"]