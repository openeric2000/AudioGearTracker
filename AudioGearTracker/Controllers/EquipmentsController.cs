using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AudioGearTracker.Core.Entities;
using AudioGearTracker.Infrastructure.Data;

namespace AudioGearTracker.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly AppDbContext _context;

        public EquipmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(string searchString)
        {
            // 1. 使用 _context 取得所有資料 (包含關聯的 Brand)
            // 注意：這裡改用 _context，並加上 Include 確保品牌名稱讀得到
            var equipments = await _context.Equipments
                                           .Include(e => e.Brand)
                                           .ToListAsync();

            // 2. 如果有搜尋字串，在記憶體中進行篩選
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                // 這裡的 Where 是針對 List 操作，支援 Enum 轉字串，非常安全
                var filteredList = equipments.Where(e =>
                    e.ModelName.ToLower().Contains(searchString) ||
                    (e.Brand != null && e.Brand.Name.ToLower().Contains(searchString)) ||
                    e.Type.ToString().ToLower().Contains(searchString)
                ).ToList();

                // 3. 儲存搜尋字串供 View 回填
                ViewData["CurrentFilter"] = searchString;

                return View(filteredList);
            }

            // 3. 如果沒搜尋，回傳全部
            ViewData["CurrentFilter"] = searchString;
            return View(equipments);
        }

        // GET: Equipments/SearchJson
        // 這是一個 API，專門回傳 JSON 給即時搜尋框使用
        [HttpGet]
        public async Task<IActionResult> SearchJson(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new List<object>());

            term = term.ToLower();

            var results = await _context.Equipments
                .Include(e => e.Brand)
                .Where(e =>
                    // 只搜尋 ModelName (型號) 和 Brand.Name (品牌)
                    e.ModelName.ToLower().Contains(term) ||
                    e.Brand.Name.ToLower().Contains(term)
                )
                .Select(e => new {
                    id = e.Id,
                    title = e.ModelName,
                    subtitle = $"{e.Brand.Name} · {e.Type}",
                    url = Url.Action("Details", "Equipments", new { id = e.Id })
                })
                .Take(8)
                .ToListAsync();

            return Json(results);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .Include(e => e.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelName,Type,Price,PurchaseDate,ReviewScore,Notes,BrandId")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Country", equipment.BrandId);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", equipment.BrandId);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,Type,Price,PurchaseDate,ReviewScore,Notes,BrandId")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Country", equipment.BrandId);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .Include(e => e.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipments.Remove(equipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }
    }
}
