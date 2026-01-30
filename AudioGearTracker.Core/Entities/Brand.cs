using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AudioGearTracker.Core.Entities;

public class Brand
{
    public int Id { get; set; }

    [Display(Name = "品牌名稱")]
    [Required(ErrorMessage = "請輸入品牌名稱")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "國家/地區")]
    public string Country { get; set; } = string.Empty;

    // 導覽屬性
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}