FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY DentalApp/*.csproj .
RUN dotnet restore

COPY DentalApp ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DentalApp.dll"]


#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
#COPY bin/Release/netcoreapp3.1/publish/ App/
#WORKDIR /App
#ENTRYPOINT ["dotnet", "DentalApp.dll"]
