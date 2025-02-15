FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

RUN export PATH=$PATH:/home/app/.dotnet/tools

ENTRYPOINT ["dotnet", "financial-control.dll"]
