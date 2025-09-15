FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MinimalApiElk.csproj", "."]
RUN dotnet restore "./MinimalApiElk.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MinimalApiElk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApiElk.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "MinimalApiElk.dll"]