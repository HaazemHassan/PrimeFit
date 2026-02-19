FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/PrimeFit.API/PrimeFit.API.csproj", "PrimeFit.API/"]
COPY ["src/PrimeFit.Application/PrimeFit.Application.csproj", "PrimeFit.Application/"]
COPY ["src/PrimeFit.Domain/PrimeFit.Domain.csproj", "PrimeFit.Domain/"]
COPY ["src/PrimeFit.Infrastructure/PrimeFit.Infrastructure.csproj", "PrimeFit.Infrastructure/"]

RUN dotnet restore "./PrimeFit.API/PrimeFit.API.csproj"

COPY src/ .
WORKDIR "/src/PrimeFit.API"
RUN dotnet build "PrimeFit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PrimeFit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeFit.API.dll"]
