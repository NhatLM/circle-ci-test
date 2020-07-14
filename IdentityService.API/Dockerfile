FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/IdentityService.Identity/IdentityService.Identity.csproj", "src/IdentityService.Identity/"]
COPY ["src/IdentityService.Admin.EntityFramework.Shared/IdentityService.Admin.EntityFramework.Shared.csproj", "src/IdentityService.Admin.EntityFramework.Shared/"]
COPY ["src/IdentityService.Admin.EntityFramework.SqlServer/IdentityService.Admin.EntityFramework.SqlServer.csproj", "src/IdentityService.Admin.EntityFramework.SqlServer/"]
COPY ["src/IdentityService.Admin.EntityFramework.PostgreSQL/IdentityService.Admin.EntityFramework.PostgreSQL.csproj", "src/IdentityService.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/IdentityService.Admin.EntityFramework.MySql/IdentityService.Admin.EntityFramework.MySql.csproj", "src/IdentityService.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/IdentityService.Identity/IdentityService.Identity.csproj"
COPY . .
WORKDIR "/src/src/IdentityService.Identity"
RUN dotnet build "IdentityService.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "IdentityService.Identity.dll"]