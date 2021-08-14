FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Commerce.Core/Commerce.Core.csproj", "Commerce.Core/"]
RUN dotnet restore "Commerce.Core/Commerce.Core.csproj"
COPY . .
WORKDIR "/src/Commerce.Core"
RUN dotnet build "Commerce.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Commerce.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Commerce.Core.dll"]
