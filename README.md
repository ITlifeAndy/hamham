# HamHam - 協作式書籤管理系統

HamHam 是一個全棧協作式書籤管理系統，包含一個基於 .NET 8 的後端 API 以及一個使用 React 開發的 Chrome 擴充功能（取代新分頁頁面）。

## 專案結構

### 後端 (`src/`)
後端採用乾淨架構 (Clean Architecture) 設計：
- **HamHam.Api**: 應用程式進入點。包含控制器 (Controllers)、中間件 (Middleware) 及相依注入 (DI) 配置。
- **HamHam.Application**: 業務邏輯層。定義介面 (Interfaces) 與應用程式服務。
- **HamHam.Domain**: 核心領域層。包含實體 (Entities)、列舉 (Enums) 與領域事件。
- **HamHam.Infrastructure**: 實作層。處理資料庫存取 (EF Core)、Redis 快取及外部 API 整合。

### 前端 (`src/extension/`)
使用 React, TypeScript 和 Tailwind CSS 開發的 Chrome 擴充功能 (Manifest V3)。
- **api/**: API 用戶端及後端通訊服務定義。
- **components/**: 可複用 UI 組件（如 Bento Grid, 分類卡片等）。
- **services/**: 前端服務（如 SignalR 同步、Chrome 鬧鐘）。
- **public/**: 靜態資源及 `manifest.json`。

### 規格文件 (`openspec/`)
包含結構化的設計過程，如提案 (Proposal)、設計文件 (Design) 及任務追蹤 (Tasks)。

## 環境需求

- .NET 8 SDK / Runtime
- Node.js (LTS) & npm
- PostgreSQL 16
- Redis 7
- Docker & Docker Compose (若選擇 Docker 部署)

## 快速開始

您可以選擇使用 Docker 快速啟動，或在 Ubuntu 上進行原生部署。

### 方法 A：使用 Docker 快速部署 (推薦開發使用)
這是最簡單的啟動方式，會自動配置資料庫與快取環境。

1. **啟動所有服務**：
   ```bash
   docker-compose up -d --build
   ```
2. **驗證狀態**：
   後端 API 將運行於 `http://localhost:5000`。
   - API 測試：http://localhost:5000/weatherforecast
   - Swagger 文件：http://localhost:5000/swagger

3. **重啟服務**
   ```bash
    docker-compose up -d
   ```
---

### 方法 B：部署至 Ubuntu 伺服器 (原生部署)
適用於生產環境或希望直接控制系統資源的場景。

1. **安裝環境**：
   ```bash
   sudo apt-get update && sudo apt-get install -y dotnet-runtime-8.0 postgresql postgresql-contrib redis-server
   ```
2. **設定資料庫**：
   ```bash
   sudo -u postgres psql -c "CREATE DATABASE hamham;"
   sudo -u postgres psql -c "ALTER USER postgres WITH PASSWORD 'postgres';"
   ```
3. **建立資料表**：
   在**本地開發機**執行以下指令產生 SQL 腳本：
   ```bash
   dotnet ef migrations script -o script.sql
   ```
   將 `script.sql` 上傳至 Ubuntu 並執行：
   ```bash
   sudo -u postgres psql -d hamham -f script.sql
   ```
   如果執行失敗,應該是Linux對於斷行符號的問題
   請嘗試執行以下這組「強力清理」指令，將檔案完全轉換為純文字 UTF-8 並移除所有 Windows 殘留符號：
    移除可能的 BOM 頭、Null 字元以及 Windows 換行符號
    ```
    tr -d '\0' < script.sql | sed '1s/^\xEF\xBB\xBF//' | sed 's/\r$//' > clean_script.sql
    ```
   
   使用清理後的檔案執行
   ```
   sudo -u postgres psql -d hamham -f clean_script.sql
   ```

4. **發佈與部署**：
   在**本地開發機**執行發佈指令，並將輸出資料夾上傳至 Ubuntu：
   ```bash
   dotnet publish src/backend/HamHam.Api/HamHam.Api.csproj -c Release -o ./publish
   #Linux 單一檔案
 dotnet publish src/HamHam.Api/HamHam.Api.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o ./publish
   ```
5. **運行 API**：
   在 Ubuntu 部署路徑下執行：

   ```bash
   dotnet HamHam.Api.dll
   ```
   
   #### 配置為 Ubuntu 背景服務 (systemd)
   為了確保 API 在重啟後自動啟動且在背景運行，請建立服務文件：
   
   1. **建立服務檔案**：
      ```bash
      sudo nano /etc/systemd/system/hamham-api.service
      ```
   2. **貼入以下內容** (請修改 `/var/www/hamham-api` 為您的實際部署路徑)：
      ```ini
      [Unit]
      Description=HamHam API Service
      After=network.target postgresql.service redis-server.service

      [Service]
      Environment=ASPNETCORE_URLS=http://*:443
      WorkingDirectory=/opt/hamham
      ExecStart=/opt/hamham/HamHam.Api
      Restart=always
      # 如果 API 崩潰，5秒後自動重啟
      RestartSec=5
      KillSignal=SIGINT
      SyslogIdentifier=hamham-api
      User=www-data
      Environment=ASPNETCORE_ENVIRONMENT=Production
      Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

      [Install]
      WantedBy=multi-user.target
      ```
   3. 給執行權限
      ```
      chmod +x /var/www/hamham-api/HamHam.Api
      ```

   4. **啟用並啟動服務**：
      ```bash
      sudo systemctl daemon-reload
      sudo systemctl enable hamham-api.service
      sudo systemctl start hamham-api.service
      ```
   5. **管理指令**：
      - 查看狀態：`sudo systemctl status hamham-api.service`
      - 查看日誌：`sudo journalctl -u hamham-api.service -f`
      - 重啟服務：`sudo systemctl restart hamham-api.service`
   
   *(建議搭配 Nginx 作為反向代理)*

---

### 前端：安裝 Chrome 擴充功能
無論後端如何部署，前端安裝流程均相同：

1. **編譯前端**：
   ```bash
   cd src/extension
   npm install
   npm run build
   ```
2. **載入至瀏覽器**：
   - 開啟 Chrome 瀏覽器，進入 `chrome://extensions/`。
   - 開啟右上角的 **開發者模式 (Developer mode)**。
   - 點擊 **載入未封裝項目 (Load unpacked)**，選擇 `src/extension/dist` 資料夾。

## 部署腳本
專案提供簡易編譯腳本可用於 Linux 環境：
```bash
chmod +x build.sh
./build.sh
```
