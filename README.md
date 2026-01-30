# AudioGearTracker

AudioGearTracker 是一個基於 ASP.NET Core MVC 的音響器材管理系統，協助用戶追蹤和管理其音響設備清單、品牌資訊及相關評論。

## 專案技術 (Tech Stack)

*   **Framework**: .NET 10
*   **Web Framework**: ASP.NET Core MVC
*   **Database**: Entity Framework Core (SQL Server)
*   **Frontend**: Razor Views, Tailwind CSS 4.0 (CDN)

## 專案架構 (Architecture)

本專案採用分層架構 (N-Layer Architecture) 設計：

1.  **AudioGearTracker (Web UI)**
    *   負責處理 HTTP 請求、控制器 (Controllers) 與視圖 (Views)。
    *   目前直接依賴於 Infrastructure 層 (待改進)。
2.  **AudioGearTracker.Core**
    *   **核心層**，不依賴其他專案。
    *   包含領域實體 (Entities): `Equipment`, `Brand`。
    *   包含介面 (Interfaces): `IRepository<T>`, `IUnitOfWork` (定義業務邏輯契約)。
    *   包含列舉 (Enums): `EquipmentType`。
3.  **AudioGearTracker.Infrastructure**
    *   **基礎設施層**，實作 Core 層的介面。
    *   包含資料庫上下文 (`AppDbContext`)。
    *   包含 Repository 實作 (`EfRepository`)。
    *   負責資料庫遷移 (Migrations)。

## 快速開始 (Quick Start)

1.  **Clone 專案**
    ```bash
    git clone https://github.com/openeric2000/AudioGearTracker.git
    cd AudioGearTracker
    ```

2.  **設定資料庫連線**
    打開 `AudioGearTracker/appsettings.json`，確認 `ConnectionStrings` 指向正確的 SQL Server 實例。

3.  **執行資料庫遷移**
    在專案根目錄執行以下命令以建立資料庫：
    ```bash
    dotnet ef database update --project AudioGearTracker.Infrastructure --startup-project AudioGearTracker
    ```

4.  **執行專案**
    ```bash
    dotnet run --project AudioGearTracker
    ```