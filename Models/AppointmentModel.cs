using System;
using System.ComponentModel.DataAnnotations;

namespace Serwis.Models
{
    public class AppointmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int CarId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public bool IsServiceCompleted { get; set; } = false;
        public string? ServiceNotes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ServiceCompletionDate { get; set; }
    }
}
