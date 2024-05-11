using lr5.Data;
using lr5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace lr5.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsDbContext _dbContext;

        public ProductsController(ProductsDbContext productDBContext)
        {
            _dbContext = productDBContext;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id is null) id = 1;
            ViewBag.Id = id;
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _dbContext.Categories.ToListAsync();
                return View(product);
            }
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", new { id = product.category_id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            ViewBag.Category = await (from c in _dbContext.Categories 
                                      where c.category_id == product.category_id 
                                      select c.category_name).FirstOrDefaultAsync();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", new { id = product.category_id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await _dbContext.Categories.ToListAsync();

            var product = await _dbContext.Products.FindAsync(id);
            var productData = new Product()
            {
                Product_id = id,
                ProductName = product.ProductName,
                ProductBrand = product.ProductBrand,
                ProcessorFrequency = product.ProcessorFrequency,
                ProductPrice = product.ProductPrice,
                RamSize = product.RamSize,
                StorageSize = product.StorageSize,
                category_id = product.category_id
            };
            return View(productData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _dbContext.Categories.ToListAsync();
                return View(product);
            }

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", new { id = product.category_id });
        }

        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _dbContext.Products.Where(p => p.category_id == categoryId).ToList();
            return PartialView("_ProductListPartial", products);
        }

        public async Task<ProductDetailsViewModel> Details(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            var category = await _dbContext.Categories
                .Where(c => c.category_id == product.category_id)
                .Select(c => c.category_name)
                .FirstOrDefaultAsync();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                CategoryName = category
            };

            return viewModel;
        }
    }
}
