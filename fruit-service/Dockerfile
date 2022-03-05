ARG NEW_RELIC__API_KEY

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim
WORKDIR /app
COPY --from=build-env /app/out .
ENV NEW_RELIC__API_KEY = ${NEW_RELIC__API_KEY}

EXPOSE 80
ENTRYPOINT ["dotnet", "fruit-service.dll"]
