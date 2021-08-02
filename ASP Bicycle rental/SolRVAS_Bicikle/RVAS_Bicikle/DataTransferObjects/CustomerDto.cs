using RVAS_Bicikle.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.DataTransferObjects
{
    public class CustomerDto
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the customer's name"), MaxLength(50)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the customer's surname"), MaxLength(50)]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Please enter the customer's age")]
        public int Age { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the customer's phone number"), MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the customer's address"), MaxLength(75)]
        public string Address { get; set; }

    }
}