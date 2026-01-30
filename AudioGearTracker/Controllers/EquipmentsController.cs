using AudioGearTracker.Core.Entities;
using AudioGearTracker.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AudioGearTracker.Controllers
{
    public class EquipmentsController : Controller
    {
        // 改用 Repository
        private readonly IEquipmentRepository _equipmentRepo;
        private readonly IRepository<Brand> _brandRepo;

        public EquipmentsController(IEquipmentRepository equipmentRepo, IRepository<Brand> brandRepo)
        {
            _equipmentRepo = equipmentRepo;
            _brandRepo = brandRepo;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Equipment> equipments;

            if (!string.IsNullOrEmpty(searchString))
            {
                // 使用 Repository 的搜尋方法 (已包含品牌 Include)
                equipments = await _equipmentRepo.SearchAsync(searchString);
                ViewData["CurrentFilter"] = searchString;
            }
            else
            {
                // 使用 Repository 的含品牌讀取方法
                equipments = await _equipmentRepo.GetAllWithBrandsAsync();
            }

            return View(equipments);
        }

        // GET: Equipments/SearchJson
        [HttpGet]
        public async Task<IActionResult> SearchJson(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new List<object>());

            var cleanTerm = term.Trim().ToLower();

            // 1. 從 Repo 撈資料
            var searchResults = await _equipmentRepo.SearchAsync(cleanTerm);

            // 2. 記憶體排序 (權重邏輯)
            var sortedResults = searchResults
                .OrderByDescending(e =>
                {
                    // 計算權重分數
                    int score = 0;
                    if (e.ModelName.Trim().StartsWith(cleanTerm, StringComparison.OrdinalIgnoreCase)) score += 100; // 開頭命中 +100分
                    if (e.Brand?.Name.Trim().StartsWith(cleanTerm, StringComparison.OrdinalIgnoreCase) ?? false) score += 50; // 品牌開頭命中 +50分
                    return score;
                })
                .ThenBy(e => e.ModelName); // 同分則照字母排

            // 3. 投影 JSON
            var jsonResult = sortedResults
                .Select(e => new {
                    id = e.Id,
                    title = e.ModelName,
                    subtitle = $"{e.Brand?.Name} · {e.Type}",
                    url = Url.Action("Details", "Equipments", new { id = e.Id })
                })
                .Take(8);

            return Json(jsonResult);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // 使用 Repository 取得單筆資料
            var equipment = await _equipmentRepo.GetByIdAsync(id.Value);

            if (equipment == null) return NotFound();

            return View(equipment);
        }

        // GET: Equipments/Create
        public async Task<IActionResult> Create()
        {
            // 使用 Brand Repository 取得品牌清單
            var brands = await _brandRepo.GetAllAsync();
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name");
            return View();
        }

        // POST: Equipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelName,Type,Price,PurchaseDate,ReviewScore,Notes,BrandId")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                // 使用 Repository 新增
                await _equipmentRepo.AddAsync(equipment);
                // Repository 通常不需手動 SaveChanges，視實作而定，若 EfRepository 有封裝 Save 則不需要
                // 這裡假設 AddAsync 內部已經呼叫了 SaveChangesAsync

                return RedirectToAction(nameof(Index));
            }

            // 驗證失敗，重新撈品牌清單
            var brands = await _brandRepo.GetAllAsync();
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name", equipment.BrandId);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var equipment = await _equipmentRepo.GetByIdAsync(id.Value);
            if (equipment == null) return NotFound();

            var brands = await _brandRepo.GetAllAsync();
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name", equipment.BrandId);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,Type,Price,PurchaseDate,ReviewScore,Notes,BrandId")] Equipment equipment)
        {
            if (id != equipment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 使用 Repository 更新
                    await _equipmentRepo.UpdateAsync(equipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EquipmentExists(equipment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var brands = await _brandRepo.GetAllAsync();
            ViewData["BrandId"] = new SelectList(brands, "Id", "Name", equipment.BrandId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var equipment = await _equipmentRepo.GetByIdAsync(id.Value);
            if (equipment == null) return NotFound();

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _equipmentRepo.GetByIdAsync(id);
            if (equipment != null)
            {
                // 使用 Repository 刪除
                await _equipmentRepo.DeleteAsync(equipment);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EquipmentExists(int id)
        {
            // 檢查是否存在，可以用 GetById 判斷
            var e = await _equipmentRepo.GetByIdAsync(id);
            return e != null;
        }
    }
}