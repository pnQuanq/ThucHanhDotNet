using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.DataContext;
using ProductManagementMVC.DTOs;
using ProductManagementMVC.Models;

namespace ProductManagementMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult UserHome()
        {
            var model = new UserDTO();
            model.Users = _context.Users.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("UserHome");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserDTO model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = model.Password;

                await _context.SaveChangesAsync();
                return RedirectToAction("UserHome");
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserHome");
        }
    }
}
