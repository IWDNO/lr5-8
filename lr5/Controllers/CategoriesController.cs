using lr5.Data;
using lr5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lr5.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ProductsDbContext _dbContext;

        public CategoriesController(ProductsDbContext productDBContext)
        {
 
            _dbContext = productDBContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var categories = await _dbContext.Categories.FindAsync(id);

            return View(categories);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();

            var category = await _dbContext.Categories.FindAsync(id);
            var productData = new Category()
            {
                category_id = id,
                category_name = category.category_name,
                category_desc = category.category_desc
            };
            return View(productData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await (from c in _dbContext.Products
                                  where c.category_id == id
                                  select c).AnyAsync() )
            {
                return RedirectToAction("Index");
            }

            var product = await _dbContext.Categories.FindAsync(id);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Categories.FindAsync(id);
            _dbContext.Categories.Remove(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
