using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize]
    public class UserServiceHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserServiceHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .Join(_context.Cars,
                      appointment => appointment.CarId,
                      car => car.Id,
                      (appointment, car) => new UserServiceHistoryViewModel
                      {
                          Id = appointment.Id,
                          CarDetails = $"{car.Brand} {car.Model} {car.Year}",
                          AppointmentDate = appointment.AppointmentDate,
                          Description = appointment.Description,
                          IsServiceCompleted = appointment.IsServiceCompleted,
                          ServiceNotes = appointment.ServiceNotes,
                          ServiceCompletionDate = appointment.ServiceCompletionDate
                      })
                .ToListAsync();

            return View(appointments);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointment = await _context.Appointments
                .Where(a => a.Id == id && a.UserId == userId)
                .Join(_context.Cars,
                      appointment => appointment.CarId,
                      car => car.Id,
                      (appointment, car) => new UserServiceHistoryViewModel
                      {
                          Id = appointment.Id,
                          CarDetails = $"{car.Brand} {car.Model} {car.Year}",
                          AppointmentDate = appointment.AppointmentDate,
                          Description = appointment.Description,
                          IsServiceCompleted = appointment.IsServiceCompleted,
                          ServiceNotes = appointment.ServiceNotes,
                          ServiceCompletionDate = appointment.ServiceCompletionDate
                      })
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
