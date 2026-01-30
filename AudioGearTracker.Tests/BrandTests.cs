using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AudioGearTracker.Core.Entities;

namespace AudioGearTracker.Tests
{
    public class BrandTests
    {
        [Fact]
        public void NewBrand_Should_Have_Empty_EquipmentList()
        {
            var brand = new Brand();

            var list = brand.Equipments;

            Assert.NotNull(list);

            Assert.Empty(list);
        }

        [Fact]
        public void Can_Add_Equipment_To_Brand()
        {
            // Arrange
            var brand = new Brand();
            var equipment = new Equipment { ModelName = "Test Headphone" };

            // Act
            brand.Equipments.Add(equipment);

            // Assert
            Assert.Single(brand.Equipments);

            // 驗證
            Assert.Equal("Test Headphone", ((List<Equipment>)brand.Equipments)[0].ModelName);
        }
    }
}
