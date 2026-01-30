using AudioGearTracker.Core.Entities;
using AudioGearTracker.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace AudioGearTracker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Equipment> Equipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 設定關聯
        modelBuilder.Entity<Equipment>()
            .HasOne(e => e.Brand)
            .WithMany(b => b.Equipments)
            .HasForeignKey(e => e.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        // === 種子資料 ===
        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Focal", Country = "France" },
            new Brand { Id = 2, Name = "Sony", Country = "Japan" },
            new Brand { Id = 3, Name = "Benchmark", Country = "USA" }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment { Id = 1, BrandId = 1, ModelName = "Utopia", Type = EquipmentType.Headphone, Price = 130000, ReviewScore = 9.8, Notes = "大烏，主力耳機" },
            new Equipment { Id = 2, BrandId = 1, ModelName = "Clear MG", Type = EquipmentType.Headphone, Price = 45000, ReviewScore = 9.0, Notes = "偏暖厚、密度高" },
            new Equipment { Id = 3, BrandId = 2, ModelName = "IER-M9", Type = EquipmentType.Headphone, Price = 28000, ReviewScore = 8.5, Notes = "監聽入耳式" },
            new Equipment { Id = 4, BrandId = 3, ModelName = "DAC3 HGC", Type = EquipmentType.DAC, Price = 60000, ReviewScore = 8.8, PurchaseDate = new DateTime(2025, 12, 25), Notes = "借用中" }
        );
    }
}
