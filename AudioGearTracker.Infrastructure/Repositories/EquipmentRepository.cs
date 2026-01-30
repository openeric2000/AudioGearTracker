using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AudioGearTracker.Core.Entities;
using AudioGearTracker.Core.Interfaces;
using AudioGearTracker.Infrastructure.Data;

namespace AudioGearTracker.Infrastructure.Repositories;

// 繼承 Generic 的 EfRepository
public class EquipmentRepository : EfRepository<Equipment>, IEquipmentRepository
{
    // 建構子：把 DbContext 傳給父類別 (EfRepository)
    public EquipmentRepository(AppDbContext context) : base(context)
    {
    }

    // 實作：取得所有器材
    public async Task<IEnumerable<Equipment>> GetAllWithBrandsAsync()
    {
        return await _context.Equipments
            .Include(e => e.Brand) // 關鍵：這裡做了 Include
            .ToListAsync();
    }

    // 實作：搜尋邏輯
    public async Task<IEnumerable<Equipment>> SearchAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return await GetAllWithBrandsAsync();

        term = term.ToLower();

        return await _context.Equipments
            .Include(e => e.Brand)
            .Where(e =>
                e.ModelName.ToLower().Contains(term) ||
                e.Brand.Name.ToLower().Contains(term)
            // 這裡可以依照需求決定要不要搜尋 Type
            )
            .ToListAsync();
    }
}
