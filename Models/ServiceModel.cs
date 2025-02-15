using System.ComponentModel.DataAnnotations;

namespace Serwis.Models
{
    public class ServiceModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImagePath { get; set; } = string.Empty;
    }
}
