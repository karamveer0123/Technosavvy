FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5007

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["technosavvy.mAPI.csproj", "technosavvy.mAPI/"]
RUN dotnet restore "technosavvy.mAPI/technosavvy.mAPI.csproj"
COPY . .
WORKDIR "/src/technosavvy.mAPI"
RUN dotnet build "technosavvy.mAPI.csproj" -c Release -o /app/build /p:UseAppHost=false

FROM build AS publish
RUN dotnet publish "technosavvy.mAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "technosavvy.mAPI.dll"]