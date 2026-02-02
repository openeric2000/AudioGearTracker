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
    public class EquipmentsControllerTests
    {
        // 模擬的 Repository
        private readonly Mock<IEquipmentRepository> _mockEqRepo;
        private readonly Mock<IRepository<Brand>> _mockBrandRepo;

        // 要測試的 Controller
        private readonly EquipmentsController _controller;

        public EquipmentsControllerTests()
        {
            // 1. 初始化 Mock 物件
            _mockEqRepo = new Mock<IEquipmentRepository>();
            _mockBrandRepo = new Mock<IRepository<Brand>>();

            // 2. 把 Mock 注入到 Controller 裡面
            _controller = new EquipmentsController(_mockEqRepo.Object, _mockBrandRepo.Object);
        }

        [Fact]
        public async Task Index_ReturnsAllEquipments_WhenSearchStringIsEmpty()
        {
            // Arrange
            // 準備兩筆假資料
            var fakeList = new List<Equipment>
            {
                new Equipment { ModelName = "Fake Headphone 1" },
                new Equipment { ModelName = "Fake DAC" }
            };

            // 設定 Mock：當呼叫 GetAllWithBrandsAsync 時，回傳上面的假資料
            _mockEqRepo.Setup(repo => repo.GetAllWithBrandsAsync())
                        .ReturnsAsync(fakeList);

            // Act
            // 呼叫 Index，參數傳 null 代表沒有搜尋
            var result = await _controller.Index(null);

            // Assert
            // 確保回傳的是 ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);

            // 確保 Model 裡的資料數量正確 (應該是 2 筆)
            var model = Assert.IsAssignableFrom<IEnumerable<Equipment>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());

            // 驗證：應該要呼叫 "取得所有資料" 的方法
            _mockEqRepo.Verify(repo => repo.GetAllWithBrandsAsync(), Times.Once);

            // 驗證：不應該呼叫 "搜尋" 的方法
            _mockEqRepo.Verify(repo => repo.SearchAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Index_CallsSearchMethod_WhenSearchStringIsProvided()
        {
            // Arrange
            string keyword = "Sony";

            // 準備假資料
            var fakeList = new List<Equipment>
            {
                new Equipment { ModelName = "Sony IER-M9" }
            };

            // 設定 Mock：只有當搜尋 "Sony" 時，才回傳假資料
            _mockEqRepo.Setup(repo => repo.SearchAsync(keyword))
                        .ReturnsAsync(fakeList);

            // Act
            // 呼叫 Index，這次有傳入關鍵字
            var result = await _controller.Index(keyword);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Equipment>>(viewResult.ViewData.Model);

            // 確認只拿到單筆資料
            Assert.Single(model);

            // 驗證：這次應該要呼叫 "SearchAsync"
            _mockEqRepo.Verify(repo => repo.SearchAsync(keyword), Times.Once);

            // 驗證：這次不應該呼叫 "GetAllWithBrandsAsync"
            _mockEqRepo.Verify(repo => repo.GetAllWithBrandsAsync(), Times.Never);
        }
    }
}