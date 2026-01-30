using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AudioGearTracker.Controllers;
using AudioGearTracker.Core.Entities;
using AudioGearTracker.Core.Interfaces;

namespace AudioGearTracker.Tests
{
    /// <summary>
    /// 針對 EquipmentsController 的單元測試
    /// 測試重點：驗證 Controller 與 Repository 之間的互動邏輯，確保在不同搜尋條件下呼叫正確的資料存取方法。
    /// </summary>
    public class EquipmentsControllerTests
    {
        // 定義 Mock 物件，用於模擬 Repository 行為
        private readonly Mock<IEquipmentRepository> _mockEqRepo;
        private readonly Mock<IRepository<Brand>> _mockBrandRepo;

        // 被測試目標 (System Under Test, SUT)
        private readonly EquipmentsController _controller;

        public EquipmentsControllerTests()
        {
            // --- Setup (初始化) ---

            // 初始化 Mock 物件
            _mockEqRepo = new Mock<IEquipmentRepository>();
            _mockBrandRepo = new Mock<IRepository<Brand>>();

            // 透過依賴注入 (DI) 將 Mock 物件注入 Controller
            _controller = new EquipmentsController(_mockEqRepo.Object, _mockBrandRepo.Object);
        }

        [Fact]
        public async Task Index_ReturnsAllEquipments_WhenSearchStringIsEmpty()
        {
            // --- Arrange (準備) ---
            // 準備預期回傳的假資料 (Mock Data)
            var fakeList = new List<Equipment>
            {
                new Equipment { ModelName = "Fake Headphone 1" },
                new Equipment { ModelName = "Fake DAC" }
            };

            // 設定 Mock 行為：當呼叫 GetAllWithBrandsAsync 時，回傳上述假資料
            _mockEqRepo.Setup(repo => repo.GetAllWithBrandsAsync())
                       .ReturnsAsync(fakeList);

            // --- Act (執行) ---
            // 執行測試目標方法，傳入 null 模擬無搜尋條件
            var result = await _controller.Index(null);

            // --- Assert (驗證) ---
            // 1. 驗證回傳型別是否為 ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);

            // 2. 驗證 Model 資料是否正確映射且數量相符
            var model = Assert.IsAssignableFrom<IEnumerable<Equipment>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

            // 3. 行為驗證 (Behavior Verification)：
            // 確保 Controller 正確呼叫了 "取得所有資料" 的方法 (應執行 1 次)
            _mockEqRepo.Verify(repo => repo.GetAllWithBrandsAsync(), Times.Once);

            // 確保 Controller 未呼叫 "搜尋" 方法 (應執行 0 次)
            _mockEqRepo.Verify(repo => repo.SearchAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Index_CallsSearchMethod_WhenSearchStringIsProvided()
        {
            // --- Arrange (準備) ---
            string keyword = "Sony";

            // 準備符合搜尋條件的假資料
            var fakeList = new List<Equipment>
            {
                new Equipment { ModelName = "Sony IER-M9" }
            };

            // 設定 Mock 行為：僅當傳入參數符合 keyword 時，才回傳假資料
            _mockEqRepo.Setup(repo => repo.SearchAsync(keyword))
                       .ReturnsAsync(fakeList);

            // --- Act (執行) ---
            // 執行測試目標方法，傳入搜尋關鍵字
            var result = await _controller.Index(keyword);

            // --- Assert (驗證) ---
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Equipment>>(viewResult.ViewData.Model);

            // 驗證回傳資料筆數 (應為 1 筆)
            Assert.Single(model);

            // 行為驗證：
            // 確保 Controller 正確呼叫了 "搜尋" 方法 (應執行 1 次)
            _mockEqRepo.Verify(repo => repo.SearchAsync(keyword), Times.Once);

            // 確保 Controller 未呼叫 "取得所有資料" 方法 (應執行 0 次)
            _mockEqRepo.Verify(repo => repo.GetAllWithBrandsAsync(), Times.Never);
        }
    }
}