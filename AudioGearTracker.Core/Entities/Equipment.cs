using AudioGearTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AudioGearTracker.Core.Entities;

public class Equipment
{
    public int Id { get; set; }

    [Display(Name = "器材型號")]
    public string ModelName { get; set; } = string.Empty;

    [Display(Name = "器材類型")]
    public EquipmentType Type { get; set; }

    [Display(Name = "購入價格")]
    [DisplayFormat(DataFormatString = "{0:N0}")] //讓數字有千分位 (1,000)
    public decimal Price { get; set; }

    [Display(Name = "購入日期")]
    [DataType(DataType.Date)] // 讓瀏覽器只顯示日期選擇器，不顯示時間
    public DateTime? PurchaseDate { get; set; }

    [Display(Name = "評分 (1-10)")]
    public double ReviewScore { get; set; }

    [Display(Name = "聽感筆記")]
    public string? Notes { get; set; }

    // Foreign Key
    [Display(Name = "品牌")]
    public int BrandId { get; set; }

    [Display(Name = "品牌")]
    public Brand? Brand { get; set; }
}
