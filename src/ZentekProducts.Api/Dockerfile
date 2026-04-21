FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ZentekProducts.Api.csproj", "./"]
RUN dotnet restore "ZentekProducts.Api.csproj"
COPY . .
RUN dotnet build "ZentekProducts.Api.csproj" -c Release -o /app/build
RUN dotnet publish "ZentekProducts.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ZentekProducts.Api.dll"]