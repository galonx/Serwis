using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serwis.Models;

namespace Serwis.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<CarModel> Cars { get; set; }
    public DbSet<CarManagerModel> CarManager { get; set; }
    public DbSet<ServiceModel> Services { get; set; }
    public DbSet<AppointmentModel> Appointments { get; set; }
}
