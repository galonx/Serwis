using System;
using System.ComponentModel.DataAnnotations;

namespace Serwis.Models
{
    public class ServiceRequestsViewModel
    {
        public int Id { get; set; }

        public string CarDetails { get; set; } = string.Empty;

        [Display(Name = "Data i godzina wizyty")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Opis problemu")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Czy serwis wykonano?")]
        public bool IsServiceCompleted { get; set; }

        [Display(Name = "Opis wykonanej usługi")]
        public string? ServiceNotes { get; set; }

        [Display(Name = "Data wykonania serwisu")]
        [DataType(DataType.DateTime)]
        public DateTime? ServiceCompletionDate { get; set; }
    }
}
