using RVAS_Bicikle.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace RVAS_Bicikle.Models
{
    public class AgeValidation : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (CustomerDto)validationContext.ObjectInstance;

            if (customer.Age <= 13)
            {
                return new ValidationResult("The service is only available for customers over the age of 13");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}