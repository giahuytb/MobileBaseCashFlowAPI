# Use the .NET Core SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy the project file
COPY *.csproj ./

# Restore the dependencies
RUN dotnet restore

# Copy the source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the .NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory
WORKDIR /app

# Copy the published files from the build-env image to the container
COPY --from=build-env /app/out .

# Install Redis client
RUN apt-get update \
    && apt-get install -y redis-tools

# Set the environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://*:$PORT
ENV REDIS_HOST=redis
ENV REDIS_PORT=6379

# Expose the port
EXPOSE $PORT

# Run the application
ENTRYPOINT ["dotnet", "MyApp.dll"]