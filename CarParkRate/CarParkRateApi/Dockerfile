#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CarParkRateApi/CarParkRateApi.csproj", "CarParkRateApi/"]
COPY ["CarParkRateInterface/CarParkRateInterface.csproj", "CarParkRateInterface/"]
COPY ["CarParkRateCommon/CarParkRateCommon.csproj", "CarParkRateCommon/"]
COPY ["CarParkRateService/CarParkRateService.csproj", "CarParkRateService/"]
COPY ["CarParkRateCore/CarParkRateCore.csproj", "CarParkRateCore/"]
RUN dotnet restore "CarParkRateApi/CarParkRateApi.csproj"
COPY . .
WORKDIR "/src/CarParkRateApi"
RUN dotnet build "CarParkRateApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarParkRateApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarParkRateApi.dll"]