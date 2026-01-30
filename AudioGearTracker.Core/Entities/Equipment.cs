using System;
using System.Collections.Generic;
using System.Text;
using AudioGearTracker.Core.Enums;

namespace AudioGearTracker.Core.Entities;

public class Equipment
{
    public int Id { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public EquipmentType Type { get; set; }
    public decimal Price { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public double ReviewScore { get; set; }
    public string? Notes { get; set; }

    // Foreign Key
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
}
