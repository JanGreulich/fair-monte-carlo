# Use the official .NET 9 SDK image (includes runtime)
FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

# Copy project file and restore dependencies
COPY FairMonteCarlo.csproj ./
RUN dotnet restore

# Copy source code
COPY . ./

# Build and publish in one step
RUN dotnet publish -c Release -o out --no-restore

# Set environment variables
ENV DOTNET_ENVIRONMENT=Production

# Set the entry point
ENTRYPOINT ["dotnet", "/app/out/FairMonteCarlo.dll"]
