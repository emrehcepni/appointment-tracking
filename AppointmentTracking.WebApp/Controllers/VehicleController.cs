using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Controllers;

public class VehicleController : Controller
{
    private readonly AppDbContext _context;

    public VehicleController(AppDbContext context)
    {
        _context = context;
    }

    // POST: Vehicle/AddOrUpdate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrUpdate(Vehicle vehicle)
    {
        if (ModelState.IsValid)
        {
            if (vehicle.VehicleId == 0)
            {
                await _context.Vehicles.AddAsync(vehicle); 
                TempData["SuccessMessage"] = "Araç başarıyla kaydedildi.";
            }
            else
            {
                var existingVehicle = await _context.Vehicles.FindAsync(vehicle.VehicleId);
                if (existingVehicle != null)
                {
                    existingVehicle.Make = vehicle.Make;
                    existingVehicle.Brand = vehicle.Brand;
                    existingVehicle.ModelYear = vehicle.ModelYear;
                    existingVehicle.LicensePlate = vehicle.LicensePlate;
                    existingVehicle.InspectionDate = vehicle.InspectionDate;
                    existingVehicle.Accessible = vehicle.Accessible;
                    _context.Vehicles.Update(existingVehicle);
                    TempData["SuccessMessage"] = "Araç başarıyla güncellendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Güncellenecek araç bulunamadı.";
                    return RedirectToAction("Index");
                }
            }

            await _context.SaveChangesAsync(); 
            Console.WriteLine($"Adding or Updating Vehicle: {vehicle.Make} {vehicle.ModelYear} ({vehicle.LicensePlate})");

            return RedirectToAction("Index");
        }
        else
        {
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    Console.WriteLine($"ModelState Hatası - {key}: {error.ErrorMessage}");
                }
            }
        }

        TempData["ErrorMessage"] = "Lütfen tüm alanları doğru doldurduğunuzdan emin olun.";
        return RedirectToAction("Index");
    }

    //index oto list
    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        ViewBag.ActiveMenuItem = "Araçlar";
        int pageSize = 5;
        int pageNumber = page ?? 1;

        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Where(item => !item.IsDeleted)
            .OrderBy(v => v.VehicleId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); 

        int totalCount = await _context.Vehicles.CountAsync(); 
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;

        return View("Index", vehicles);
    }

    // Filtre
    [HttpGet]
    public async Task<IActionResult> Search(int? page, string? make, string? brand, int? modelYear, string? licensePlate, DateTime? inspectionDate, bool? accessible)
    {
        int pageSize = 5;
        int pageNumber = page ?? 1;

        var vehicles = _context.Vehicles
            .Where(v => v.IsDeleted == false) // ❗ Silinmemiş araçları getiriyoruz
            .AsQueryable();

        // Arama kriterleri varsa uygula
        if (!string.IsNullOrEmpty(make))
            vehicles = vehicles.Where(v => v.Make.Contains(make));

        if (!string.IsNullOrEmpty(brand))
            vehicles = vehicles.Where(v => v.Brand.Contains(brand));

        if (modelYear.HasValue)
            vehicles = vehicles.Where(v => v.ModelYear == modelYear.Value);

        if (!string.IsNullOrEmpty(licensePlate))
            vehicles = vehicles.Where(v => v.LicensePlate.Contains(licensePlate));

        if (inspectionDate.HasValue)
            vehicles = vehicles.Where(v => v.InspectionDate == inspectionDate.Value);

        if (accessible.HasValue)
            vehicles = vehicles.Where(v => v.Accessible == accessible.Value);

        int totalCount = await vehicles.CountAsync(); // Asenkron sayım
        var pagedVehicles = await vehicles
            .OrderBy(v => v.VehicleId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(); // **Burada listeye çeviriyoruz!**

        // ViewBag'e sayfa bilgilerini ekleyelim
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;
        ViewBag.Make = make;
        ViewBag.Brand = brand;
        ViewBag.ModelYear = modelYear;
        ViewBag.LicensePlate = licensePlate;
        ViewBag.InspectionDate = inspectionDate;
        ViewBag.Accessible = accessible;

        return View("Index", pagedVehicles); // **Burada artık bir List<Vehicle> gidiyor!**
    }

    //public IActionResult Search(int? page, string make, string brand, int? modelYear, string licensePlate, DateTime inspectionDate, bool? accessible)
    //{
    //    int pageSize = 5;
    //    int pageNumber = page ?? 1;

    //    var vehicles = _context.Vehicles.AsQueryable();

    //    // Arama kriterleri varsa uygula
    //    if (!string.IsNullOrEmpty(make))
    //        vehicles = vehicles.Where(v => v.Make.Contains(make));

    //    if (!string.IsNullOrEmpty(brand))
    //        vehicles = vehicles.Where(v => v.Brand.Contains(brand));

    //    if (modelYear.HasValue)
    //        vehicles = vehicles.Where(v => v.ModelYear == modelYear.Value);

    //    if (!string.IsNullOrEmpty(licensePlate))
    //        vehicles = vehicles.Where(v => v.LicensePlate.Contains(licensePlate));

    //    if (inspectionDate != null)
    //    {
    //        vehicles = vehicles.Where(v => v.InspectionDate == inspectionDate);
    //    }



    //    if (accessible.HasValue)
    //        vehicles = vehicles.Where(v => v.Accessible == accessible.Value);

    //    int totalCount = vehicles.Count();
    //    var pagedVehicles = vehicles
    //        .OrderBy(v => v.VehicleId)
    //        .Skip((pageNumber - 1) * pageSize)
    //        .Take(pageSize)
    //        .ToList();

    //    ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    //    ViewBag.CurrentPage = pageNumber;
    //    ViewBag.Make = make;
    //    ViewBag.Brand = brand;
    //    ViewBag.ModelYear = modelYear;
    //    ViewBag.LicensePlate = licensePlate;
    //    ViewBag.InspectionDate = inspectionDate;
    //    ViewBag.Accessible = accessible;

    //    return View("Index", pagedVehicles);
    //}

    public IActionResult CheckInspectionStatus()
    {
        DateTime today = DateTime.Today;
        DateTime warningDate = today.AddDays(10);

        var vehiclesWithExpiringInspection = _context.Vehicles
            .Where(v => v.InspectionDate.HasValue && v.InspectionDate.Value <= warningDate)
            .ToList();

        return Json(vehiclesWithExpiringInspection);
    }

    // POST: Vehicle/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int selectedVehicleId)
    {
        var vehicle = await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(item => item.VehicleId == selectedVehicleId);
        if (vehicle is null)
        {
            TempData["ErrorMessage"] = "Araç bulunamadı.";
            return RedirectToAction("Index");
        }

        vehicle.IsDeleted = true;
        _context.Vehicles.Update(vehicle);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Araç başarıyla silindi.";

        return RedirectToAction("Index");
    }
}
