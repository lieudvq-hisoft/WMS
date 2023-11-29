#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Development TZ=Asia/Ho_Chi_Minh

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WMS/WMS.csproj", "WMS/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "WMS/WMS.csproj"
COPY . .
WORKDIR "/src/WMS"
RUN dotnet build "WMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WMS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WMS.dll"]
