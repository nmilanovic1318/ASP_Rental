using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{
    public class Repair
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date of repair")]
        public DateTime DateOfRepair { get; set; }

        public virtual Bicycle Bicycle { get; set; }

        [Display(Name = "Bicycle Model")]
        public int BicycleId { get; set; }

        [Required]
        public float Price { get; set; }


    }
}