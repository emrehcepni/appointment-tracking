using Microsoft.AspNetCore.Mvc;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Services.Interfaces;

namespace AppointmentTracking.Controllers;

public class CandidateController : Controller
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }


    // GET: Candidate/Index
    public async Task<IActionResult> Index(int? page, string searchString)
    {
        ViewBag.ActiveMenuItem = "Adaylar";
        int pageSize = 2;
        int pageNumber = page ?? 1;

        var candidates = (await _candidateService.GetAllCandidates()).ToList();

        if (!string.IsNullOrEmpty(searchString))
        {
            candidates = candidates
                .Where(c =>
                    c.FirstName.Contains(searchString) ||
                    c.LastName.Contains(searchString) ||
                    c.PhoneNumber.Contains(searchString))
                .ToList();
        }

        int totalCount = candidates.Count();

        var pagedCandidates = candidates
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.CurrentPage = pageNumber;
        ViewBag.SearchString = searchString;

        return View(pagedCandidates);
    }

    // POST: Candidate/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Candidate candidate)
    {
        if (ModelState.IsValid)
        {
            await _candidateService.AddCandidate(candidate);
            return RedirectToAction("Index");
        }

        foreach (var modelState in ModelState.Values)
        {
            foreach (var error in modelState.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

        var candidates = (await _candidateService.GetAllCandidates()).ToList();
        return View("Index", candidates);
    }

    // POST: Candidate/Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid selectedCandidateId)
    {
        if (selectedCandidateId == Guid.Empty)
        {
            TempData["ErrorMessage"] = "Lütfen silmek için bir aday seçin.";
            return RedirectToAction("Index");
        }

        var candidate = await _candidateService.GetCandidateById(selectedCandidateId);
        if (candidate is null)
        {
            TempData["ErrorMessage"] = "Aday bulunamadı.";
            return RedirectToAction("Index");
        }

        await _candidateService.DeleteCandidate(candidate.Id); 
        TempData["SuccessMessage"] = "Aday başarıyla silindi.";
        return RedirectToAction("Index");
    }

}