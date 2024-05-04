FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish -c release -o /build --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /build

ENTRYPOINT ["dotnet", "osz2-service.dll"]