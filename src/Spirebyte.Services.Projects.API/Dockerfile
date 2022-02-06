#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Project-Service/src/Spirebyte.Services.Projects.API/Spirebyte.Services.Projects.API.csproj", "src/Project-Service/src/Spirebyte.Services.Projects.API/"]
COPY ["src/Project-Service/src/Spirebyte.Services.Projects.Infrastructure/Spirebyte.Services.Projects.Infrastructure.csproj", "src/Project-Service/src/Spirebyte.Services.Projects.Infrastructure/"]
COPY ["src/Project-Service/src/Spirebyte.Services.Projects.Application/Spirebyte.Services.Projects.Application.csproj", "src/Project-Service/src/Spirebyte.Services.Projects.Application/"]
COPY ["src/Project-Service/src/Spirebyte.Services.Projects.Core/Spirebyte.Services.Projects.Core.csproj", "src/Project-Service/src/Spirebyte.Services.Projects.Core/"]
RUN dotnet restore "src/Project-Service/src/Spirebyte.Services.Projects.API/Spirebyte.Services.Projects.API.csproj"
COPY . .
WORKDIR "/src/src/Project-Service/src/Spirebyte.Services.Projects.API"
RUN dotnet build "Spirebyte.Services.Projects.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Projects.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Projects.API.dll"]
