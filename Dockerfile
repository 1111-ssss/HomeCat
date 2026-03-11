FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Web.sln
RUN dotnet publish Web/Web.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

COPY ./init-db.sql ./init-db.sql

ENTRYPOINT ["sh", "-c", "dotnet Web.dll & sleep 5 && sqlite3 homecat.db < init-db.sql && wait"]