FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FundingSouq.Assessment.Api/FundingSouq.Assessment.Api.csproj", "FundingSouq.Assessment.Api/"]
COPY ["FundingSouq.Assessment.Application/FundingSouq.Assessment.Application.csproj", "FundingSouq.Assessment.Application/"]
COPY ["FundingSouq.Assessment.Core/FundingSouq.Assessment.Core.csproj", "FundingSouq.Assessment.Core/"]
COPY ["FundingSouq.Assessment.Infrastructure/FundingSouq.Assessment.Infrastructure.csproj", "FundingSouq.Assessment.Infrastructure/"]
RUN dotnet restore "FundingSouq.Assessment.Api/FundingSouq.Assessment.Api.csproj"
COPY . .
WORKDIR "/src/FundingSouq.Assessment.Api"
RUN dotnet build "FundingSouq.Assessment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FundingSouq.Assessment.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FundingSouq.Assessment.Api.dll"]
