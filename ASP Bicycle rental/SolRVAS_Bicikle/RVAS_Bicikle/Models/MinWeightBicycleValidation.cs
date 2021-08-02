using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.Models
{

    // Validacija težine prilikom ubacivanja nove bicikle u bazu; dozvoljene samo vrednosti preko 2.7
    public class MinWeightBicycleValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bicycle = (Bicycle)validationContext.ObjectInstance;
            return (bicycle.Weight >= 2.7) ? ValidationResult.Success : new ValidationResult("Please enter a valid weight for the bicycle");
        }
    }
}