using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme i�in gerekli
using YemekTarifleriUygulamasi.Models;

namespace YemekTarifleriUygulamasi.Controllers
{
    [Authorize] // Bu controller alt�ndaki t�m aksiyonlara yetkilendirme zorunlulu�u ekler
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

        // Gizlilik politikas�
        public IActionResult Privacy()
        {
            return View();
        }

        // Hata sayfas�
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
