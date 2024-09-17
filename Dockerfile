FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN . /etc/os-release \
    && case $ID in \
        alpine) apk add --no-cache python3 ;; \
        debian | ubuntu) apt-get update \
            && apt-get install -y --no-install-recommends python3 \
            && rm -rf /var/lib/apt/lists/* ;; \
        mariner) tdnf install -y python3 \
            && tdnf clean all ;; \
        esac \
    && dotnet workload install --skip-manifest-update wasm-tools

WORKDIR /app
COPY . ./repo

RUN dotnet restore ./repo/src/Client/ActivityPaint.Client.Web/ActivityPaint.Client.Web.csproj
# RUN dotnet build ./repo/src/Client/ActivityPaint.Client.Web/ActivityPaint.Client.Web.csproj --no-restore -c Release

FROM build AS publish

RUN dotnet publish ./repo/src/Client/ActivityPaint.Client.Web/ActivityPaint.Client.Web.csproj -r linux-x64 -c Release --self-contained true -o /app/publish

FROM nginx:alpine AS runner

WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .
# COPY --from=publish /app/publish/e_sqlite3.a ./_framework/e_sqlite3.a
COPY nginx.conf /etc/nginx/nginx.conf
