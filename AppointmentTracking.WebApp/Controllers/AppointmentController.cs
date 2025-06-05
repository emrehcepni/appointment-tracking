using AppointmentTracking.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.CustomExtensions;
using AppointmentTracking.Services.Interfaces;
using AppointmentTracking.Services;

namespace AppointmentTracking.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;
    private readonly ICandidateService _candidateService;
    private readonly IInstructorService _instructorService;
    private readonly IVehicleService _vehicleService;
    public AppointmentController(IAppointmentService appointmentService, ICandidateService candidateService, IInstructorService instructorService, IVehicleService vehicleService)
    {
        _appointmentService = appointmentService;
        _candidateService = candidateService;
        _instructorService = instructorService;
        _vehicleService = vehicleService;
    }

    public async Task<IActionResult> Index(int? month, int? week, Guid? instructorId, Guid? vehicleId)
    {
        ViewBag.ActiveMenuItem = "Randevu";
        ViewBag.Candidates = await _candidateService.GetAllCandidates();
        ViewBag.Instructors = await _instructorService.GetAllInstructors();
        ViewBag.Vehicles = await _vehicleService.GetAllVehicles();

        var appointments = await _appointmentService.GetAppointments(month, week, instructorId, vehicleId);
        return View(appointments);
    }

    //public JsonResult GetAvailableCandidates(DateTime startTime)
    //{
    //    var bookedCandidates = _context.Appointments
    //        .Where(a => a.StartTime == startTime)
    //        .Select(a => a.CandidateId)
    //        .ToList();

    //    var availableCandidates = _context.Candidates
    //        .Where(c => !bookedCandidates.Contains(c.CandidateId))
    //        .ToList();

    //    return Json(availableCandidates);
    //}

    //public JsonResult GetAvailableVehicles(DateTime startTime)
    //{
    //    // Randevusu yapılmış araçları filtreleyin
    //    var bookedVehicles = _context.Appointments
    //        .Where(a => a.StartTime == startTime)  // Verilen tarihte ve saatteki randevular
    //        .Select(a => a.VehicleId)  // Sadece araç ID'lerini seçiyoruz
    //        .ToList();  // Randevusu olan araçları listele

    //    // bookedVehicles listesini kontrol et
    //    Console.WriteLine("Booked Vehicles (Randevusu olan araçlar): " + string.Join(", ", bookedVehicles));

    //    // Randevusu olmayan ve erişilebilir araçları filtreleyin
    //    var availableVehicles = _context.Vehicles
    //        .Where(v => !bookedVehicles.Contains(v.VehicleId) && v.Accessible)  // Randevusu olmayan ve erişilebilir araçları alıyoruz
    //        .Select(v => new
    //        {
    //            v.VehicleId,
    //            v.LicensePlate
    //        })
    //        .ToList();  // Verileri belleğe al

    //    // AvailableVehicles listesini kontrol et
    //    Console.WriteLine("Available Vehicles (Randevusu olmayan araçlar): " + string.Join(", ", availableVehicles.Select(v => v.LicensePlate)));

    //    // Eğer bookedVehicles listesi boşsa, yani hiç randevu yoksa, tüm araçlar alınır.
    //    if (!bookedVehicles.Any())
    //    {
    //        availableVehicles = _context.Vehicles
    //            .Where(v => v.Accessible)  // Erişilebilir araçları alıyoruz
    //            .Select(v => new
    //            {
    //                v.VehicleId,
    //                v.LicensePlate
    //            })
    //            .ToList();

    //        // Tüm araçları kontrol et (Erişilebilir olanlar)
    //        Console.WriteLine("All Available Vehicles (Tüm araçlar - Erişilebilir): " + string.Join(", ", availableVehicles.Select(v => v.LicensePlate)));
    //    }

    //    return Json(availableVehicles);
    //}
//    public JsonResult GetAllVehicles()
//    {
//        // Tüm araçları alıyoruz
//        var allVehicles = _context.Vehicles
//            .AsNoTracking()  // Değişiklik yapmayacağımız için performansı artırmak için AsNoTracking kullanıyoruz
//            .Select(v => new
//            {
//                v.VehicleId,
//                v.LicensePlate,
//                v.Accessible
//            })
//            .ToList();  // Tüm araçları listele

//        // Araçları kontrol et (console)
//        Console.WriteLine("All Vehicles: " + string.Join(", ", allVehicles.Select(v => v.LicensePlate)));

//        return Json(allVehicles);  // Tüm araçları döndürüyoruz
//    }

//    [HttpGet]
//    public JsonResult GetAppointments(int month, int week, int instructorId)
//    {
//        var startOfWeek = new DateTime().GetWeekStartDate(DateTime.Now.Year, month, week);
//        var endOfWeek = startOfWeek.AddDays(6);

//        var appointments = _context.Appointments
//            .Include(a => a.Instructor)
//            .Include(a => a.Candidate)
//            .Include(a => a.Vehicle)
//            .Where(a => a.StartTime.Date >= startOfWeek && a.StartTime.Date <= endOfWeek)
//            .Where(a => a.InstructorId == instructorId)
//            .Select(a => new
//            {
//                a.StartTime,
//                Candidate = a.Candidate != null ? a.Candidate.FirstName + " " + a.Candidate.LastName : "Boş",
//                Vehicle = a.Vehicle != null ? a.Vehicle.LicensePlate : "Plaka Yok"
//            })
//            .ToList();

//        return Json(appointments);
//    }

//    [HttpPost]
//    public JsonResult SaveAppointment(int instructorId, DateTime startTime, int candidateId, int vehicleId)
//    {
//        var candidate = _context.Candidates.FirstOrDefault(c => c.CandidateId == candidateId);
//        var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == vehicleId);

//        if (candidate == null || vehicle == null)
//        {
//            return Json(new { success = false, message = "Aday veya araç bulunamadı!" });
//        }

//        var appointment = _context.Appointments
//            .FirstOrDefault(a => a.StartTime == startTime && a.InstructorId == instructorId);

//        if (appointment != null)
//        {
//            appointment = new Appointment
//            {
//                InstructorId = instructorId,
//                StartTime = startTime,
//                CandidateId = candidateId,
//                VehicleId = vehicle.VehicleId
//            };

//            _context.Appointments.Add(appointment);
//        }
//        else
//        {
//            appointment.CandidateId = candidateId;
//            appointment.VehicleId = vehicle.VehicleId;
//        }

//        _context.SaveChanges();
//        return Json(new { success = true });
//    }

//    public JsonResult GetAvailableDates(int instructorId, int month, int week)
//    {
//        // Örnek veri oluşturma
//        var weekDates = new List<string>
//{
//    "2025-03-18",
//    "2025-03-19",
//    "2025-03-20",
//    "2025-03-21"
//};
//        var hours = new List<string>
//{
//    "09:00", "10:00", "11:00", "12:00"
//};

//        return Json(new { success = true, weekDates = weekDates, hours = hours });
//    }

    //public JsonResult SaveAppointment(int instructorId, DateTime startTime, int candidateId, string vehiclePlate)
    //{
    //    var candidate = _context.Candidates.FirstOrDefault(c => c.CandidateId == candidateId);
    //    var vehicle = _context.Vehicles.FirstOrDefault(v => v.LicensePlate == vehiclePlate);

    //    if (candidate == null || vehicle == null)
    //    {
    //        return Json(new { success = false, message = "Aday veya araç bulunamadı!" });
    //    }

    //    var appointment = _context.Appointments
    //        .FirstOrDefault(a => a.StartTime == startTime && a.InstructorId == instructorId);

    //    if (appointment == null)
    //    {
    //        appointment = new Appointment
    //        {
    //            InstructorId = instructorId,
    //            StartTime = startTime,
    //            CandidateId = candidateId,
    //            VehicleId = vehicle.VehicleId
    //        };

    //        _context.Appointments.Add(appointment);
    //    }
    //    else
    //    {
    //        appointment.CandidateId = candidateId;
    //        appointment.VehicleId = vehicle.VehicleId;
    //    }

    //    _context.SaveChanges();
    //    return Json(new { success = true });
    //}
}
