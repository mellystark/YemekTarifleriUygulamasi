using Microsoft.AspNetCore.Mvc;
using YemekTarifleriUygulamasi.Data;
using YemekTarifleriUygulamasi.Models;
using BCrypt.Net;

namespace YemekTarifleriUygulamasi.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Hash the password before saving
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kullanıcıyı veritabanından al
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                // Şifreyi kontrol et (hash karşılaştırması)
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    // Başarılı giriş
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Şifre hatalı
                    ViewBag.ErrorMessage = "Invalid password.";
                    return View();
                }
            }
            else
            {
                // Kullanıcı bulunamadı
                ViewBag.ErrorMessage = "User not found.";
                return View();
            }
        }
    }
}
