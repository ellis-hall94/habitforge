FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HabitForge.Api/HabitForge.Api.csproj HabitForge.Api/
RUN dotnet restore HabitForge.Api/HabitForge.Api.csproj

COPY . .
RUN dotnet publish HabitForge.Api/HabitForge.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HabitForge.Api.dll"]