using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{
    public class Rental
    {

        [Required]
        [Key]
        public int Id { get; set; }

        public virtual Bicycle Bicycle { get; set; }

        [Required]
        [ForeignKey("Bicycle")]
        [Display(Name = "Bicycle model")]
        public int BicycleId { get; set; }


        public virtual Customer Customer { get; set; }

        [Required]
        [ForeignKey("Customer")]
        [Display(Name = "Customer name")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter the rental date")]
        [Display(Name = "Date of rental")]
        public DateTime DateRented { get; set; }

        [Display(Name = "Date of return")]
        public DateTime? DateReturned { get; set; }



    }
}