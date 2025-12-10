# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Test.Api/Test.Api.csproj", "Test.Api/"]
COPY ["Test.Application/Test.Application.csproj", "Test.Application/"]
COPY ["Test.Infrastructure/Test.Infrastructure.csproj", "Test.Infrastructure/"]
COPY ["Test.Domain/Test.Domain.csproj", "Test.Domain/"]
RUN dotnet restore "Test.Api/Test.Api.csproj"
COPY . .
WORKDIR "/src/Test.Api"
RUN dotnet build "Test.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.Api.dll"]
