FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsolePhotoAlbum/ConsolePhotoAlbum.csproj", "ConsolePhotoAlbum/"]
RUN dotnet restore "ConsolePhotoAlbum/ConsolePhotoAlbum.csproj"
COPY . .
WORKDIR "/src/ConsolePhotoAlbum"
RUN dotnet build "ConsolePhotoAlbum.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsolePhotoAlbum.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsolePhotoAlbum.dll"]
