using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Infrastructure;
using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Controllers;

public class InstructorController : Controller
{
    private readonly AppDbContext _context;
    IConfiguration _configuration;
    public InstructorController(AppDbContext context ,IConfiguration configuration)//bak
    {
        _context = context;
        _configuration = configuration;
    }

    // GET: Instructor/Index
    public IActionResult Index(string searchString)
    {
        ViewBag.ActiveMenuItem = "Eğitmenler";
        Console.WriteLine(ViewBag.ActiveMenuItem);
        var instructors = _context.Instructors.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            instructors = instructors.Where(i =>
                i.FirstName.Contains(searchString) ||
                i.LastName.Contains(searchString) ||
                i.PhoneNumber.Contains(searchString) ||
                i.LicanceType.Contains(searchString));
        }

        return View(instructors.ToList());
    }

    // POST: Instructor/AddOrUpdate
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrUpdate(Instructor instructor, string[] LicanceType)
    {
        if (instructor != null)
        {
            instructor.LicanceType = string.Join(",", LicanceType); // Çoklu seçimleri string'e çeviriyoruz

            if (instructor.Id == Guid.Empty)
            {
                _context.Instructors.Add(instructor);
                TempData["SuccessMessage"] = "Eğitmen başarıyla eklendi.";
            }
            else
            {
                var existingInstructor = _context.Instructors.Find(instructor.Id);
                if (existingInstructor != null)
                {
                    existingInstructor.FirstName = instructor.FirstName;
                    existingInstructor.LastName = instructor.LastName;
                    existingInstructor.PhoneNumber = instructor.PhoneNumber;
                    existingInstructor.LicanceType = instructor.LicanceType;
                    _context.Instructors.Update(existingInstructor);
                    TempData["SuccessMessage"] = "Eğitmen başarıyla güncellendi.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Güncellenecek eğitmen bulunamadı.";
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
        return RedirectToAction("Index");
        
    }


    // POST: Instructor/Delete
    [HttpPost]
    public IActionResult Delete(int instructorId)
    {
        var instructor = _context.Instructors.Find(instructorId);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Eğitmen başarıyla silindi.";
        }
        else
        {
            TempData["ErrorMessage"] = "Silinecek eğitmen bulunamadı.";
        }
        return RedirectToAction("Index");
    }
}
