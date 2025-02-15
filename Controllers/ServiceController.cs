using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Serwis.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var services = new List<ServiceModel>
            {
                new ServiceModel { Id = 1, Name = "AutoFix24", Address = "Ul. Główna", Description = "Serwis specjalizujący się w naprawach samochodów osobowych. Zapewniamy szybką i profesjonalną obsługę. Dzięki wieloletniemu doświadczeniu oraz nowoczesnemu sprzętowi gwarantujemy usługi na najwyższym poziomie.", ImagePath = "/images/service1.png" },
                new ServiceModel { Id = 2, Name = "MotoCare", Address = "Ul. Przemysłowa", Description = "Serwis dedykowany samochodom ciężarowym. Oferujemy kompleksowe usługi naprawcze oraz diagnostyczne dla pojazdów o dużej masie. Posiadamy specjalistyczny sprzęt oraz zespół wykwalifikowanych mechaników.", ImagePath = "/images/service2.png" },
                new ServiceModel { Id = 3, Name = "CarService Pro", Address = "Ul. Uniwersalna", Description = "Kompleksowy serwis dla wszystkich rodzajów pojazdów – osobowych, ciężarowych i dostawczych. Niezależnie od typu pojazdu, możesz liczyć na pełną obsługę, od diagnostyki po naprawy mechaniczne.", ImagePath = "/images/service3.png" }
            };

            return View(services);
        }

        [HttpPost]
        public IActionResult SelectService(int serviceId)
        {
            Console.WriteLine($"Wybrany serwis ID: {serviceId}");

            if (serviceId <= 0)
            {
                Console.WriteLine("Nieprawidłowy ID serwisu.");
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("SelectedServiceId", serviceId);
            return RedirectToAction("Book");
        }

        [Authorize]
        public async Task<IActionResult> Book()
        {
            var serviceId = HttpContext.Session.GetInt32("SelectedServiceId");

            if (serviceId == null)
            {
                Console.WriteLine("Brak zapisanego ID serwisu w sesji. Przekierowanie do Index.");
                return RedirectToAction("Index");
            }

            Console.WriteLine($"ID serwisu pobrane z sesji: {serviceId}");

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                Console.WriteLine($"Serwis o ID {serviceId} nie znaleziony w bazie.");
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            Console.WriteLine($"Pobrane UserId w ServiceController: {userId}");

            var userCars = await _context.Cars.Where(c => c.UserId == userId).ToListAsync();

            ViewData["SelectedService"] = service;

            var viewModel = new AppointmentViewModel
            {
                ServiceId = service.Id,
                UserId = userId,
                UserCars = userCars
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookPost(AppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid!");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                var service = await _context.Services.FindAsync(model.ServiceId);
                var userCars = await _context.Cars.Where(c => c.UserId == model.UserId).ToListAsync();

                ViewData["SelectedService"] = service;
                var viewModel = new AppointmentViewModel
                {
                    ServiceId = model.ServiceId,
                    UserId = model.UserId,
                    UserCars = userCars
                };

                return View("Book", viewModel);
            }

            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
