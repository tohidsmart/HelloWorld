using System;
using System.ComponentModel.DataAnnotations;

namespace HelloWorld.ViewModels
{
    public class StopAPIModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255,MinimumLength =5)]
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        public DateTime Arrival { get; set; }
    }
}