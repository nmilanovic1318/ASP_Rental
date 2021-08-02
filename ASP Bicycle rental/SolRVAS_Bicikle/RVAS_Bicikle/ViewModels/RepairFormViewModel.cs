using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RVAS_Bicikle.Models;

namespace RVAS_Bicikle.ViewModels
{
    public class RepairFormViewModel
    {
        public IEnumerable<Bicycle> Bicycles { get; set; }
        public Repair Repair { get; set; }
    }
}