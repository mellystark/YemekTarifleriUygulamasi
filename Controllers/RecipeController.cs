using Microsoft.AspNetCore.Mvc;
using YemekTarifleriUygulamasi.Data;
using YemekTarifleriUygulamasi.Models;
using System.IO;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YemekTarifleriUygulamasi.Controllers
{
    public class RecipeController : Controller
    {
        private readonly AppDbContext _context;

        public RecipeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Recipe/Index
        public IActionResult Index()
        {
            var recipes = _context.Recipes.ToList();
            if (!recipes.Any())
            {
                ViewBag.Message = "Henüz tarif eklenmemiş.";
            }
            return View(recipes);
        }

        // GET: Recipe/Details/5
        public IActionResult Details(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // Kategoriler listesi
        private List<string> GetCategories()
        {
            return new List<string>
            {
                "Tatlı", "Vejetaryen", "Çorba", "Aperatif", "Tavuk", "Salata",
                "Makarna", "Et", "Fast Food", "Meze",
            };
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            // Kategoriler listesini ViewBag ile gönderme
            var categories = GetCategories();
            ViewBag.Categories = new SelectList(categories);

            // Eğer kategoriler boşsa, hata ver
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Kategoriler yüklenemedi.");
            }

            return View();
        }

        // POST: Recipe/Create
        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe, IFormFile Image)
        {
            // Kategoriler listesini ViewBag ile gönderme
            var categories = GetCategories();
            ViewBag.Categories = new SelectList(categories, recipe.Category);

            // Eğer kategoriler boşsa, hata ver
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Kategoriler yüklenemedi.");
                return View(recipe);
            }

            if (ModelState.IsValid)
            {
                // Görsel yükleme işlemi
                if (Image != null && Image.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(Image.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Sadece jpg, jpeg, png ve gif formatındaki dosyalara izin verilir.");
                        return View(recipe);
                    }

                    var directory = Path.Combine("wwwroot", "images", "recipes");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(directory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(stream);
                    }

                    recipe.ImagePath = $"/images/recipes/{fileName}";
                }

                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Kategoriler tekrar gönder
            ViewBag.Categories = new SelectList(categories, recipe.Category);
            return View(recipe);
        }



        // GET: Recipe/Edit/5
        public IActionResult Edit(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Kategoriler listesini ViewBag ile gönderme
            ViewBag.Categories = new SelectList(GetCategories(), recipe.Category);
            return View(recipe);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Recipe recipe, IFormFile Image)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingRecipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
                if (existingRecipe == null)
                {
                    return NotFound();
                }

                try
                {
                    // Görsel güncelleme işlemi
                    if (Image != null && Image.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(Image.FileName).ToLower();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("", "Sadece jpg, jpeg, png ve gif formatındaki dosyalara izin verilir.");
                            return View(recipe);
                        }

                        var directory = Path.Combine("wwwroot", "images", "recipes");
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(directory, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            Image.CopyTo(stream);
                        }

                        // Yeni görseli ekle
                        recipe.ImagePath = $"/images/recipes/{fileName}";
                    }
                    else
                    {
                        // Görsel değiştirilmiyorsa eski görseli koru
                        recipe.ImagePath = existingRecipe.ImagePath;
                    }

                    // Diğer alanları güncelle
                    existingRecipe.Title = recipe.Title;
                    existingRecipe.Description = recipe.Description;
                    existingRecipe.Ingredients = recipe.Ingredients;
                    existingRecipe.Steps = recipe.Steps;
                    existingRecipe.Category = recipe.Category;
                    existingRecipe.ImagePath = recipe.ImagePath;

                    _context.Recipes.Update(existingRecipe);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Görsel güncellenirken bir hata oluştu: {ex.Message}");
                }
            }

            // Kategorileri tekrar gönder
            ViewBag.Categories = new SelectList(GetCategories(), recipe.Category);
            return View(recipe);
        }


        // GET: Recipe/Delete/5
        public IActionResult Delete(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null)
            {
                // Görsel dosyasını sil
                if (!string.IsNullOrEmpty(recipe.ImagePath))
                {
                    var filePath = Path.Combine("wwwroot", recipe.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
