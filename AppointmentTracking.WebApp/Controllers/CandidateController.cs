using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Controllers;

public class CandidateController : Controller
{
    private readonly AppDbContext _context;

    public CandidateController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Candidate/Index
    public IActionResult Index(int? page, string searchString)
    {
        ViewBag.ActiveMenuItem = "Adaylar";
        int pageSize = 2; // Her sayfada gösterilecek kayıt sayısı
        int pageNumber = page ?? 1; // Sayfa numarası (varsayılan: 1)

        // Adayları filtrele
        var candidates = _context.Candidates.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            candidates = candidates.Where(c =>
                c.FirstName.Contains(searchString) ||
                c.LastName.Contains(searchString) ||
                c.PhoneNumber.Contains(searchString));
        }

        // Toplam kayıt sayısını hesapla
        int totalCount = candidates.Count();

        // Sayfalama yap
        var pagedCandidates = candidates
            .OrderBy(c => c.Id) // Sıralama yap
            .Skip((pageNumber - 1) * pageSize) // Atlanacak kayıt sayısı
            .Take(pageSize) // Alınacak kayıt sayısı
            .ToList();

        // ViewBag ile sayfalama bilgilerini View'a gönder
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;
        ViewBag.SearchString = searchString; // Arama metnini View'da tut

        return View(pagedCandidates);
    }

    // POST: Candidate/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Candidate candidate)
    {
        if (ModelState.IsValid)
        {
            _context.Candidates.Add(candidate);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        foreach (var modelState in ModelState.Values)
        {
            foreach (var error in modelState.Errors)
            {
                Console.WriteLine(error.ErrorMessage); 
            }
        }
        

        // Eğer model geçerli değilse, mevcut aday listesi ile sayfayı tekrar göster
        var candidates = _context.Candidates.ToList();
        return View("Index", candidates);
    }

    // POST: Candidate/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int selectedCandidateId)
    {
        if (selectedCandidateId == 0)
        {
            TempData["ErrorMessage"] = "Lütfen silmek için bir aday seçin.";
            return RedirectToAction("Index");
        }
        var candidate = await _context.Candidates.AsNoTracking().FirstOrDefaultAsync(item => item.Id == Guid.Empty); // selectedCandidateId
        if (candidate is null)
        {
            TempData["ErrorMessage"] = "Aday bulunamadı.";
            return RedirectToAction("Index");
        }

        candidate.IsDeleted = true;
        _context.Candidates.Update(candidate);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Araç başarıyla silindi.";
        return RedirectToAction("Index");
    }
}