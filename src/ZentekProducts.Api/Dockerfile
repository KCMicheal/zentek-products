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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZentekProducts.Api.dll"]