FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /OlympusExample

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /OlympusExample
COPY --from=build-env /OlympusExample/out .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "OlympusExample.dll"]