# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install dependencies for SkiaSharp (QuestPDF)
RUN apt-get update && apt-get install -y libfontconfig1

# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Test.Web/Test.Web.csproj", "Test.Web/"]
COPY ["Test.Application/Test.Application.csproj", "Test.Application/"]
COPY ["Test.Infrastructure/Test.Infrastructure.csproj", "Test.Infrastructure/"]
COPY ["Test.Domain/Test.Domain.csproj", "Test.Domain/"]
RUN dotnet restore "Test.Web/Test.Web.csproj"
COPY . .
WORKDIR "/src/Test.Web"
RUN dotnet build "Test.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test.Web.dll"]
