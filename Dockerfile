# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5090

# Use the .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["recipe-finder/recipe-finder.csproj", "recipe-finder/"]
RUN dotnet restore "recipe-finder/recipe-finder.csproj"
COPY . .
WORKDIR "/src/recipe-finder"
RUN dotnet build "recipe-finder.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "recipe-finder.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "recipe-finder.dll"]

#push