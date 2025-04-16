
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BookLibrary.API/BookLibrary.API.csproj", "src/BookLibrary.API/"]
COPY ["src/BookLibrary.Application/BookLibrary.Application.csproj", "src/BookLibrary.Application/"]
COPY ["src/BookLibrary.Domain/BookLibrary.Domain.csproj", "src/BookLibrary.Domain/"]
COPY ["src/BookLibrary.Infrastructure/BookLibrary.Infrastructure.csproj", "src/BookLibrary.Infrastructure/"]
RUN dotnet restore "./src/BookLibrary.API/BookLibrary.API.csproj"
COPY . .
WORKDIR "/src/src/BookLibrary.API"
RUN dotnet build "./BookLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.API.dll"]