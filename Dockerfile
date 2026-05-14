# 1. Aşama: Çalışma zamanı (Runtime) imajı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# 2. Aşama: Derleme (Build) imajı
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece Solution ve Proje dosyalarını kopyalayarak 'restore' işlemini cache'liyoruz
# Bu, kod değişikliklerinde restore işleminin tekrar etmesini önleyerek hızı artırır
COPY ["SharedPool.sln", "./"]
COPY ["SharedPool.API/SharedPool.API.csproj", "SharedPool.API/"]
COPY ["SharedPool.Application/SharedPool.Application.csproj", "SharedPool.Application/"]
COPY ["SharedPool.Domain/SharedPool.Domain.csproj", "SharedPool.Domain/"]
COPY ["SharedPool.Infrastructure/SharedPool.Infrastructure.csproj", "SharedPool.Infrastructure/"]

RUN dotnet restore "SharedPool.sln"

# Kalan tüm dosyaları kopyala
COPY . .

# Doğrudan API klasörüne odaklanıyoruz
WORKDIR "/src/SharedPool.API"
RUN dotnet build "SharedPool.API.csproj" -c Release -o /app/build

# 3. Aşama: Yayınlama (Publish)
FROM build AS publish
RUN dotnet publish "SharedPool.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Aşama: Final imajı oluşturma
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SharedPool.API.dll"]