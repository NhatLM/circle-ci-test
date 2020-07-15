# Build 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
WORKDIR /build/IdentityService.API
RUN dotnet build "IdentityService.API.csproj" -c Release -o /app
RUN dotnet publish -c release -r debian-x64 -o /app

# Run
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS http://+:80
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]