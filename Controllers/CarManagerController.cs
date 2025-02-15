using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class CarManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allCars = await _context.Cars.ToListAsync();

            var carManagerList = allCars.Select(car => new CarManagerModel
            {
                Id = car.Id,
                UserId = car.UserId,
                VIN = car.VIN,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                EngineType = car.EngineType,
                Capacity = car.Capacity
            }).ToList();

            return View(carManagerList);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carManager = new CarManagerModel
            {
                Id = car.Id,
                UserId = car.UserId,
                VIN = car.VIN,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                EngineType = car.EngineType,
                Capacity = car.Capacity
            };

            return View(carManager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, CarManagerModel carManager)
        {
            if (id != carManager.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(carManager);
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            car.Brand = carManager.Brand;
            car.Model = carManager.Model;
            car.Year = carManager.Year;
            car.EngineType = carManager.EngineType;
            car.Capacity = carManager.Capacity;
            car.UserId = carManager.UserId;

            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carManager = new CarManagerModel
            {
                Id = car.Id,
                UserId = car.UserId,
                VIN = car.VIN,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                EngineType = car.EngineType,
                Capacity = car.Capacity
            };

            return View(carManager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            try
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                Console.WriteLine($"{car.VIN} zostało usunięte!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
