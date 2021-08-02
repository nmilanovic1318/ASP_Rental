using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RVAS_Bicikle.Models;

namespace RVAS_Bicikle.ViewModels
{
    public class BicycleFormViewModel
    {
        public IEnumerable<Brand> Brands { get; set; }
        public Bicycle Bicycle { get; set; }
    }
}