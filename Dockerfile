# 1. .NET runtime ortamını kullanıyoruz (Sadece çalıştırma için)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# 2. .NET SDK ortamını kullanarak uygulamayı build edeceğiz
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UserManagementApp.sln", "./"]
COPY ["UserManagementApp/", "UserManagementApp/"]
COPY ["UserManagementApp.Business/", "UserManagementApp.Business/"]
COPY ["UserManagementApp.DataAccess/", "UserManagementApp.DataAccess/"]
COPY ["UserManagementApp.Entities/", "UserManagementApp.Entities/"]

RUN dotnet restore "UserManagementApp/UserManagementApp.csproj"

# 3. Proje dosyalarını kopyala ve derle
COPY . .
WORKDIR "/src/UserManagementApp"
RUN dotnet build "UserManagementApp.csproj" -c Release -o /app/build

# 4. Yayın için publish işlemini yap
FROM build AS publish
RUN dotnet publish "UserManagementApp.csproj" -c Release -o /app/publish

# 5. Çalıştırılabilir ortamı oluştur ve uygulamayı başlat
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagementApp.dll"]
