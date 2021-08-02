using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{


    public class Bicycle
    {

        [Required]
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the frame material"), MaxLength(30)]
        public string Frame { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the bicycle's model"), MaxLength(25)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please enter the bicycle's weight")]
        [MinWeightBicycleValidation]
        public float Weight { get; set; }
        [Required(ErrorMessage = "Please enter the bicycle's price")]
        public float Price { get; set; }

        public Brand Brand { get; set; }

        [Required]
        [ForeignKey("Brand")]
        [Display(Name = "Brand name")]
        public int BrandId { get; set; }

        [Required]
        public bool TrainingWheels { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }
    }
}