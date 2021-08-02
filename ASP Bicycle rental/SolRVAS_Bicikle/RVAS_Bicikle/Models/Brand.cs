using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{

    public class Brand
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the brand name"), MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Bicycle> Bicycles { get; set; }
    }
}