using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using YemekTarifleriUygulamasi.Data;
using YemekTarifleriUygulamasi.Models;
using BCrypt.Net;
using System.Security.Claims;

namespace YemekTarifleriUygulamasi.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AppDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Register sayfası yüklendi.");
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Şifreyi hashleyerek kaydet
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    _logger.LogInformation("Yeni kullanıcı kaydedildi: {Username}", user.Username);

                    // Kayıttan sonra Login sayfasına yönlendir
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kullanıcı kaydedilirken bir hata oluştu.");
                    ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.");
                }
            }
            else
            {
                _logger.LogWarning("Geçersiz model verisi ile kayıt denemesi.");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login sayfası yüklendi.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            _logger.LogInformation("Login işlemi başlatıldı. Kullanıcı adı: {Username}", username);

            // Kullanıcıyı veritabanından al
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                // Şifre doğrulaması yap
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    _logger.LogInformation("Kullanıcı giriş yaptı: {Username}", username);

                    // Kimlik doğrulaması ve kullanıcıyı oturum açtırma
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Oturumu başlat
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Hatalı şifre ile giriş denemesi. Kullanıcı adı: {Username}", username);
                    ViewBag.ErrorMessage = "Geçersiz şifre.";
                }
            }
            else
            {
                _logger.LogWarning("Geçersiz kullanıcı adı ile giriş denemesi: {Username}", username);
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Kullanıcı çıkış yaptı.");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
