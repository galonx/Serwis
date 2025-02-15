using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Book(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userCars = await _context.Cars.Where(c => c.UserId == userId).ToListAsync();

            var viewModel = new AppointmentViewModel
            {
                Service = service,
                UserCars = userCars
            };

            return View(viewModel);
        }
    }
}
