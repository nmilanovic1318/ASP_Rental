using RVAS_Bicikle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RVAS_Bicikle.ViewModels;

namespace RVAS_Bicikle.Controllers
{
    public class RepairsController : Controller
    {
        private ApplicationDbContext _context;

        // instanciranje DBContexta kroz konstruktor
        public RepairsController()
        {
            _context = new ApplicationDbContext();
        }

        // Prevencija curenja memorije
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Services
        // Metoda za prikaz podataka o popravkama; ako nije u ulozi "Administrator" dobija samo ReadOnly listu
        [Authorize(Roles = RoleName.Administrator + "," + RoleName.User)]
        public ActionResult Index()
        {
            var repairs = _context.Repairs.Include(b => b.Bicycle).Include(r => r.Bicycle.Brand).ToList();
            if (!User.IsInRole(RoleName.Administrator))
            {
                return View("ReadOnlyRepairsList", repairs);
            }
            else
            {
                return View(repairs);
            }

        }

        // Prikaz detalja o popravci; dozvoljena za korisnike u ulozi "Administrator" i "User"
        [Authorize(Roles = RoleName.Administrator + "," + RoleName.User)]
        public ActionResult Details(int id)
        {
            var repair = _context.Repairs.Include(b => b.Bicycle).Include(r => r.Bicycle.Brand).SingleOrDefault(r => r.Id == id);
            if (repair == null)
            {
                return HttpNotFound();
            }
            return View(repair);
        }


        // Metoda za prikaz forme za dodavanje popravke; dozvoljena samo korisnicima u ulozi "Administrator"
        [HttpGet]
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Create()
        {

            var bicycles = _context.Bicycles.ToList();
            var serviceViewModel = new RepairFormViewModel()
            {
                Repair = new Repair(),
                Bicycles = bicycles
            };
            return View("RepairForm", serviceViewModel);
        }


        // Metoda za ubacivanje (ili izmenu) podataka o popravci u DB; dozvoljena samo korisnicima u ulozi "Administrator"
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Store(Repair repair)
        {
            // Provera da li je sve u redu sa modelom, validacija; u slučaju da nešto nije u redu, vraća View popunjen onako kako je bio u trenutku exceptiona
            if (!ModelState.IsValid)
            {
                var bicycles = _context.Bicycles.ToList();
                var repairViewModel = new RepairFormViewModel
                {
                    Repair = repair,
                    Bicycles = bicycles
                };
                return View("RepairForm", repairViewModel);
            }
            if (repair.Id == 0)
            {
                _context.Repairs.Add(repair);
            }
            else
            {
                var ExistingRepair = _context.Repairs.SingleOrDefault(r => r.Id == repair.Id);
                ExistingRepair.Bicycle = _context.Bicycles.SingleOrDefault(r => r.Id == repair.BicycleId);
                ExistingRepair.Price = repair.Price;
                ExistingRepair.DateOfRepair = repair.DateOfRepair;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Repairs");
        }


        // Metoda za prikaz forme za izmenu podataka o popravci
        [HttpGet]
        [Route("Repairs/Edit")]
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Edit(int id)
        {
            var repair = _context.Repairs.SingleOrDefault(r => r.Id == id);

            if (repair == null)
            {
                return HttpNotFound();
            }

            var repairViewModel = new RepairFormViewModel()
            {
                Repair = repair,
                Bicycles = _context.Bicycles.ToList()

            };
            return View("RepairForm", repairViewModel);
        }


        // Metoda za brisanje popravke iz baze
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult DeleteRepair(int id)
        {
            var repairToDelete = _context.Repairs.FirstOrDefault(r => r.Id == id);
            _context.Repairs.Remove(repairToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Repairs");
        }
    }
}