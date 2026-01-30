using AudioGearTracker.Core.Interfaces;
using AudioGearTracker.Infrastructure.Data;
using AudioGearTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 設定 DB Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 註冊 Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// 註冊專屬的 EquipmentRepository
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 預設
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();