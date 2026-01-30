using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AudioGearTracker.Core.Entities;
using AudioGearTracker.Core.Interfaces; // 引用介面

namespace AudioGearTracker.Controllers;

public class BrandsController : Controller
{
    // 改用 Repository 介面，而不是直接依賴 DbContext
    private readonly IRepository<Brand> _repository;

    public BrandsController(IRepository<Brand> repository)
    {
        _repository = repository;
    }

    // GET: Brands
    public async Task<IActionResult> Index()
    {
        // 使用 Repository 的方法
        return View(await _repository.GetAllAsync());
    }

    // GET: Brands/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var brand = await _repository.GetByIdAsync(id.Value);
        if (brand == null) return NotFound();

        return View(brand);
    }

    // GET: Brands/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Brands/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Country")] Brand brand)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddAsync(brand);
            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }

    // GET: Brands/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var brand = await _repository.GetByIdAsync(id.Value);
        if (brand == null) return NotFound();
        return View(brand);
    }

    // POST: Brands/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country")] Brand brand)
    {
        if (id != brand.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _repository.UpdateAsync(brand);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.GetByIdAsync(brand.Id) == null)
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(brand);
    }

    // GET: Brands/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var brand = await _repository.GetByIdAsync(id.Value);
        if (brand == null) return NotFound();

        return View(brand);
    }

    // POST: Brands/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var brand = await _repository.GetByIdAsync(id);
        if (brand != null)
        {
            await _repository.DeleteAsync(brand);
        }
        return RedirectToAction(nameof(Index));
    }
}