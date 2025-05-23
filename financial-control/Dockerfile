FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5044

ENV ASPNETCORE_URLS=http://+:5044

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["financial-control/financial-control.csproj", "financial-control/"]
RUN dotnet restore "financial-control/financial-control.csproj"
COPY . .
WORKDIR "/src/financial-control"
RUN dotnet build "financial-control.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "financial-control.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "financial-control.dll"]
