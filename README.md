# 🎵 Audio Gear Tracker

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?style=flat-square)
![EF Core](https://img.shields.io/badge/EF%20Core-ORM-blue?style=flat-square)
![Tests](https://img.shields.io/badge/Tests-xUnit-success?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-lightgrey?style=flat-square)

> 一個採用 **N-Tier 分層架構** 與 **Clean Architecture 原則** 打造的現代化器材管理系統。本專案展示了如何在傳統 MVC 架構中，完美融合 Web API 功能與 SOLID 設計模式。

---

## 📖 專案簡介 (Introduction)

**AudioGearTracker** 是一個專為音樂與錄音器材愛好者設計的庫存管理解決方案。

與傳統的 MVC 專案不同，本系統採用 **Hybrid 架構設計**：

1.  **Server-Side Rendering (SSR)**：核心頁面使用 Razor Views 渲染，確保 SEO 與首次載入效能。
2.  **Modern API Integration**：整合了 JSON API Endpoints (`SearchJson`)，支援前端透過 AJAX 進行無刷新即時搜尋 (Live Search)，展示了現代化的使用者體驗。

本專案旨在演示企業級應用程式的標準開發實務，包含依賴注入 (DI)、單元測試 (Unit Testing) 與關注點分離 (SoC)。

---

## 🏗️ 系統架構 (System Architecture)

本專案遵循 **分層架構 (N-Tier Architecture)**，嚴格控管層與層之間的依賴方向，確保核心邏輯 (Core) 不受外部實作 (Infrastructure/Web) 的影響。

### 專案結構 (Project Structure)

```text
AudioGearTracker/
|
|--- AudioGearTracker/                   # Presentation Layer (ASP.NET Core MVC)
|    |--- Controllers/                   # MVC 控制器 & API Endpoints
|    |--- Views/                         # Razor 視圖檔案
|    |--- Models/                        # 視圖模型 (ViewModels)
|    |--- wwwroot/                       # 靜態資源 (CSS, JS, Libs)
|    |--- Program.cs                     # 應用程式進入點 & DI 配置
|
|--- AudioGearTracker.Core/              # Core Layer (核心商業邏輯 & 抽象)
|    |--- Entities/                      # 領域實體 (Domain Entities)
|    |--- Enums/                         # 列舉類型
|    |--- Interfaces/                    # 介面定義 (Repository Interfaces)
|
|--- AudioGearTracker.Infrastructure/    # Infrastructure Layer (資料存取實作)
|    |--- Data/                          # 資料庫上下文 (DbContext)
|    |--- Migrations/                    # EF Core 遷移檔案
|    |--- Repositories/                  # 倉儲實作 (Repository Implementation)
|
|--- AudioGearTracker.Tests/             # Test Layer (單元測試)
     |--- BrandTests.cs                  # 品牌功能測試
     |--- EquipmentsControllerTests.cs   # 控制器測試
```

- **Core Layer (核心層)**: 系統的核心，包含 `Entities` (資料模型) 與 `Interfaces` (合約)。不依賴任何其他層。
- **Infrastructure Layer (基礎設施層)**: 負責資料存取。實作了 Core 層定義的介面 (Repository Pattern) 並透過 EF Core 與 SQL Server溝通。
- **Presentation Layer (表現層)**: 也就是 Web MVC 專案。透過 `Dependency Injection` 取得服務，不直接依賴 `DbContext`，實現鬆散耦合。

---

## 🛠️ 技術堆疊 (Tech Stack)

### Backend & Core

- **Framework**: .NET 10 (ASP.NET Core MVC)
- **Language**: C#
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core (EF Core) Code-First
- **Dependency Injection**: Built-in Container

### Frontend

- **Stying**: Tailwind CSS (Utility-first CSS)
- **Scripting**: JavaScript (Vanilla JS for AJAX fetch)
- **View Engine**: Razor

### Testing

- **Framework**: xUnit
- **Mocking**: Moq (用於模擬 Repository 行為，隔離資料庫依賴)

---

## 💡 設計模式與開發亮點 (Key Features & Patterns)

### 1. Repository Pattern (倉儲模式)

- **目的**: 將商業邏輯與資料存取邏輯解耦。
- **實作**: Controller 僅依賴 `IGearRepository` 介面，而非具體的 `EfGearRepository` 或 `DbContext`。這使得未來更換資料庫或進行測試變得極其容易。

### 2. Live Search API (即時搜尋)

- **目的**: 提升使用者滿意度，避免傳統 MVC 每次搜尋都要重新載入整頁。
- **實作**: 實現了一個 Hybrid Controller Action，能夠根據請求回傳 JSON 格式的搜尋結果，供前端 JavaScript 動態渲染。

### 3. Dependency Injection (依賴注入)

- **目的**: 實現 Inversion of Control (IoC)。
- **實作**: 在 `Program.cs` 中註冊服務生命週期：
    ```csharp
    builder.Services.AddScoped<IGearRepository, GearRepository>();
    ```

### 4. Code-First Migration

- 資料庫結構完全由 C# 類別 (Entities) 定義與管理，確保版控的一致性。

---

## 🚀 如何執行專案 (Getting Started)

請確認您的環境已安裝 **.NET 8 SDK** 與 **SQL Server LocalDB**。

### 1. Clone 專案

```bash
git clone https://github.com/YourUsername/AudioGearTracker.git
cd AudioGearTracker
```

### 2. 設定資料庫 (Update-Database)

本專案使用 EF Core Migrations。在專案根目錄開啟終端機：

```bash
# 還原套件
dotnet restore

# 應用資料庫遷移 (這會自動建立 LocalDB 資料庫)
dotnet ef database update --project AudioGearTracker.Infrastructure --startup-project AudioGearTracker.Web
```

_(註：若您將所有層寫在同一個專案，只需執行 `dotnet ef database update`)_

### 3. 執行網站

```bash
dotnet run --project AudioGearTracker.Web
```

打開瀏覽器訪問 `https://localhost:5001` (或終端機顯示的 Port)。

### 4. 執行測試

```bash
dotnet test
```

---
