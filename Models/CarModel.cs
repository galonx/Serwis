using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Serwis.Models
{
    public class CarModel
    {
        [Key]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Numer VIN")]
        public string VIN { get; set; }

        [Required]
        [Display(Name = "Marka")]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Rocznik")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Typ silnika")]
        public string EngineType { get; set; }

        [Required]
        [Display(Name = "Pojemność silnika")]
        public double Capacity { get; set; }
    }
}
