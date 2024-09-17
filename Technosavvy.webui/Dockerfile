FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Technosavvy.webui.csproj", "Technosavvy.webui/"]
RUN dotnet restore "Technosavvy.webui/Technosavvy.webui.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "Technosavvy.webui.csproj" -c Release -o /app/build /p:UseAppHost=false

FROM build AS publish
RUN dotnet publish "Technosavvy.webui.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Technosavvy.webui.dll"]