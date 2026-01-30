using System;
using System.Collections.Generic;
using System.Text;
using AudioGearTracker.Core.Entities;

namespace AudioGearTracker.Core.Interfaces;

// 繼承原本的 IRepository<Equipment>
public interface IEquipmentRepository : IRepository<Equipment>
{
    // 新增一個專門用來讀取「品牌資料」的方法
    Task<IEnumerable<Equipment>> GetAllWithBrandsAsync();

    // 新增一個專門處理「搜尋」的方法
    Task<IEnumerable<Equipment>> SearchAsync(string term);
}
