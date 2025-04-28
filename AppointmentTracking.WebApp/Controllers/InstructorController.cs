using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Infrastructure;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Controllers;

public class InstructorController : Controller
{
    private readonly AppDbContext _context;
    private readonly IInstructorService _instructorService;
    
    public InstructorController(AppDbContext context, IInstructorService instructorService)
    {
        _context = context;
        _instructorService = instructorService;
    }

    // GET: Instructor/Index
    public async Task<IActionResult> Index(string searchString)
    {
        ViewBag.ActiveMenuItem = "Eğitmenler";
        
        if (string.IsNullOrEmpty(searchString))
            return View(new List<Instructor>());
        
        var instructors = await _instructorService.SearchInstructors(searchString);
        return View(instructors);
    }

    // POST: Instructor/AddOrUpdate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrUpdate(Instructor instructor, string[] LicanceType)
    {
        if (instructor is null)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
            return RedirectToAction("Index");
        }
        
        instructor.LicanceType = string.Join(",", LicanceType); // Çoklu seçimleri string'e çeviriyoruz

        if (instructor.Id == Guid.Empty)
        {
            await _instructorService.AddInstructor(instructor);
            
            TempData["SuccessMessage"] = "Eğitmen başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        
        var existingInstructor = await _instructorService.GetInstructorById(instructor.Id);
        if (existingInstructor is null)
        {
            TempData["ErrorMessage"] = "Güncellenecek eğitmen bulunamadı.";
            return RedirectToAction("Index");
        }

        existingInstructor.FirstName = instructor.FirstName;
        existingInstructor.LastName = instructor.LastName;
        existingInstructor.PhoneNumber = instructor.PhoneNumber;
        existingInstructor.LicanceType = instructor.LicanceType;
        
        await _instructorService.UpdateInstructor(existingInstructor);
        
        TempData["SuccessMessage"] = "Eğitmen başarıyla güncellendi.";
        return RedirectToAction("Index");
    }

    // POST: Instructor/Delete
    [HttpPost]
    public async Task<IActionResult> Delete(Guid instructorId)
    {
        var result = await _instructorService.DeleteInstructor(instructorId);
        
        if (result)
            TempData["SuccessMessage"] = "Eğitmen başarıyla silindi.";
        else
            TempData["ErrorMessage"] = "Silinecek eğitmen bulunamadı.";
        
        return RedirectToAction("Index");
    }
}
