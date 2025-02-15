using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Serwis.Models;

namespace Serwis.Models
{
    public class AppointmentViewModel
    {
        public string UserId { get; set; }
        public ServiceModel Service { get; set; } = new ServiceModel();
        public List<CarModel> UserCars { get; set; } = new List<CarModel>();

        [Required]
        public int CarId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int ServiceId { get; set; }
    }
}
