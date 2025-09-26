# Use official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore dependencies
COPY *.sln ./
COPY MarketPlace.Web/*.csproj MarketPlace.Web/
COPY MarketPlace.Infrastructure/*.csproj MarketPlace.Infrastructure/
COPY MarketPlace.Domain/*.csproj MarketPlace.Domain/
COPY MarketPlace.Application/*.csproj MarketPlace.Application/

RUN dotnet restore MarketPlace.Web/MarketPlace.Web.csproj

# Copy everything and build the app
COPY . ./
WORKDIR /app/MarketPlace.Web
RUN dotnet publish -c Release -o /publish

# Use a runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Expose the port your API runs on
EXPOSE 5000
EXPOSE 5001

# Define the entry point
ENTRYPOINT ["dotnet", "MarketPlace.Web.dll"]