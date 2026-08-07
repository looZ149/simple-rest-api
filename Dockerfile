FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Some Caching trick
# CSPROJ files rarely change. Only if we add/remove NuGet packages

# COPY SampleTracker/SampleTracker SampleTracker/
# RUN dotnet restore SampleTracker/SampleTracker.csproj
# RUN dotnet publish SampleTracker/SampleTracker.csproj -c Release -o /app --no-restore

# We copy the whole source code, fixing some typo in a source file would change the whole content hash, 
# meaning to also redownload the whole packages

COPY SampleTracker/SampleTracker.csproj SampleTracker/
RUN dotnet restore SampleTracker/SampleTracker.csproj

COPY SampleTracker/ SampleTracker/
RUN dotnet publish SampleTracker/SampleTracker.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "SampleTracker.dll"]

