using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"User ID in Index: {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userCars = await _context.Cars.Where(c => c.UserId == userId).ToListAsync();
            return View(userCars);
        }

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var car = new CarModel { UserId = userId };
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarModel car)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            car.UserId = userId;
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (car == null || car.UserId != userId)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarModel car)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != car.Id || car.UserId != userId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(car);
            }

            _context.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
