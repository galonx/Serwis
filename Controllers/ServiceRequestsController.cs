using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serwis.Data;
using Serwis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.Appointments
                .Join(_context.Cars,
                      appointment => appointment.CarId,
                      car => car.Id,
                      (appointment, car) => new ServiceRequestsViewModel
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

            return View(requests);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> UpdateServiceStatus(int id, bool isCompleted, string? serviceNotes, DateTime? completionDate)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsServiceCompleted = isCompleted;
            appointment.ServiceNotes = serviceNotes;
            appointment.ServiceCompletionDate = completionDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
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
