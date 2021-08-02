using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{

    public class Customer
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
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the customer's address"), MaxLength(75)]
        public string Address { get; set; }

        // svojstvo za prikaz u DropDown listi za rentale
        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public virtual ICollection<Rental> Rentals { get; set; }

    }
}