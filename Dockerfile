FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/Spirebyte.Services.Projects.API -c release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT dotnet Spirebyte.Services.Projects.API.dll
