using System;

namespace Serwis.Models
{
    public class UserServiceHistoryViewModel
    {
        public int Id { get; set; }
        public string CarDetails { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public bool IsServiceCompleted { get; set; }
        public string? ServiceNotes { get; set; }
        public DateTime? ServiceCompletionDate { get; set; }
    }
}
