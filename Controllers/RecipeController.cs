using Microsoft.AspNetCore.Mvc;
using YemekTarifleriUygulamasi.Data;
using YemekTarifleriUygulamasi.Models;
using System.Linq;

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

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
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
            return View(recipe);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Recipes.Update(recipe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
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
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Başarılı bir işlemden sonra Index'e yönlendir
        }

    }
}
