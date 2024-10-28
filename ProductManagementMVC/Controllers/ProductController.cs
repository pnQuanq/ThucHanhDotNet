using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.DataContext;
using ProductManagementMVC.DTOs;
using ProductManagementMVC.Models;

namespace ProductManagementMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult ProductHome()
        {
            var model = new ProductDTO();
            model.Products = _context.Products.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO model)
        {
            // Handling Image Upload
            string uniqueFileName = null;
            if (model.PictureFile != null && model.PictureFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                
                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PictureFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PictureFile.CopyToAsync(fileStream);
                }
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                ProductCode = model.ProductCode,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,
                Picture = uniqueFileName
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("ProductHome");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new CreateProductDTO
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                Quantity = product.Quantity,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateProductDTO model)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Handling Image Upload
                string uniqueFileName = null;
                if (model.PictureFile != null && model.PictureFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                    Directory.CreateDirectory(uploadsFolder);

                    // Delete the old image file if it exists
                    if (!string.IsNullOrEmpty(product.Picture))
                    {
                        var oldFilePath = Path.Combine(uploadsFolder, product.Picture);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.PictureFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PictureFile.CopyToAsync(fileStream);
                    }

                    product.Picture = uniqueFileName; // Update the product's Picture property
                }

                product.ProductName = model.ProductName;
                product.ProductCode = model.ProductCode;
                product.UnitPrice = model.UnitPrice;
                product.Quantity = model.Quantity;

                await _context.SaveChangesAsync();
                return RedirectToAction("ProductHome");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(product.Picture))
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                var oldFilePath = Path.Combine(uploadsFolder, product.Picture);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProductHome");
        }
    }
}
