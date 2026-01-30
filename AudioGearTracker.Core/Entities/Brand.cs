using System;
using System.Collections.Generic;
using System.Text;

namespace AudioGearTracker.Core.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // 導覽屬性
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}