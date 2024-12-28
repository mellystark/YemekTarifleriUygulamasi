using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme için gerekli
using YemekTarifleriUygulamasi.Models;

namespace YemekTarifleriUygulamasi.Controllers
{
    [Authorize] // Bu controller altýndaki tüm aksiyonlara yetkilendirme zorunluluðu ekler
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Ana sayfa
        public IActionResult Index()
        {
            return View();
        }

        // Gizlilik politikasý
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata sayfasý
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
