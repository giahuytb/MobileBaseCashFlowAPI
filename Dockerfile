# Set the working directory
WORKDIR /app

# Copy the project file
COPY *.csproj ./

# Restore the dependencies
RUN dotnet restore

## Copy the source code
#COPY . ./
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /app
#
## copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore
#
## copy everything else and build app
#COPY . ./
#RUN dotnet publish -c Release -o out
#
## build runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /app
#COPY --from=build /app/out .
#ENTRYPOINT ["dotnet", "MobileBasedCashFlowAPI.dll"]