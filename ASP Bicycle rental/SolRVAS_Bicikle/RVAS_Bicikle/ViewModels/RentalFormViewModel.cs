using RVAS_Bicikle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RVAS_Bicikle.ViewModels
{
    public class RentalFormViewModel
    { 
       
        public List<Customer> Customers { get; set; }
        public Rental Rental { get; set; }
        public List<Bicycle> Bicycles { get; set; }

                
    }

}