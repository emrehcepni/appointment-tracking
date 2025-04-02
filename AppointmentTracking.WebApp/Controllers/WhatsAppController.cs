using Microsoft.AspNetCore.Mvc;

public class WhatsAppController : Controller
{
    private readonly WhatsAppService _whatsAppService;
    public IActionResult Index()
    {
        return View();
    }
    public WhatsAppController()
    {
        _whatsAppService = new WhatsAppService();
    }

    [HttpPost]
    public IActionResult SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            ViewBag.Message = "Mesaj alanı boş olamaz.";
            return View("Index");
        }

        string result = _whatsAppService.SendMessage(message);
        ViewBag.Message = result;

        return View("Index"); // Formun olduğu sayfaya geri dön
    }
}
