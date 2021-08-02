using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RVAS_Bicikle.Models;
using RVAS_Bicikle.ViewModels;

namespace RVAS_Bicikle.Controllers
{
    public class RentalsController : Controller
    {
        private ApplicationDbContext _context;


        // instanciranje DBContexta kroz konstruktor
        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        // prevencija curenja memorije
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Rentals
        // Metoda za prikaz svih rentala; ako korisnik nije u ulozi "Administrator" dobija ReadOnlyRentals view, u suprotnom dobija Index view gde su omogućene CRUD operacije
        public ActionResult Index()
        {
            var rentals = _context.Rentals.Include(b => b.Bicycle.Brand).ToList();
            if (!User.IsInRole(RoleName.Administrator))
            {
                return View("ReadOnlyRentals", rentals);
            }

            return View(rentals);
        }

        // Metoda dozvoljena samo korisnicima u ulozi "Administrator", prikaz forme za ubacivanje nove mušterije
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Create()
        {
            var customers = _context.Customers.ToList();
            var bicycles = _context.Bicycles.ToList();
            var rentalViewModel = new RentalFormViewModel()
            {
                Customers = customers,
                Bicycles = bicycles,
                Rental = new Rental()
            };
            return View("RentalForm", rentalViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Store(Rental rental)
        {
            // Provera da li je sve u redu sa modelom; u slučaju da nešto nije u redu, vraća View popunjen onako kako je bio u trenutku exceptiona
            var bicycleRentalToAdd = _context.Bicycles.SingleOrDefault(b => b.Id == rental.BicycleId);
            var customerRentalToAdd = _context.Customers.SingleOrDefault(c => c.Id == rental.CustomerId);
            if (!ModelState.IsValid)
            {
                var customers = _context.Customers.ToList();
                var bicycles = _context.Bicycles.ToList();

                var rentalViewModel = new RentalFormViewModel()
                {
                    Customers = customers,
                    Bicycles = bicycles,
                    Rental = new Rental()
                };
                return View("RentalForm", rentalViewModel);
            }
            if (rental.Id == 0)
            {
                rental.DateRented = DateTime.Now;
                _context.Rentals.Add(rental);
                bicycleRentalToAdd.Rentals.Add(rental);
                customerRentalToAdd.Rentals.Add(rental);
            }
            else
            {
                var ExistingRental = _context.Rentals.SingleOrDefault(r => r.Id == rental.Id);
                ExistingRental.BicycleId = rental.BicycleId;
                ExistingRental.CustomerId = rental.CustomerId;
                ExistingRental.DateRented = rental.DateRented;
                ExistingRental.DateReturned = rental.DateReturned;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Rentals");
        }

        // Metoda za prikaz detalja pojedinačnog rentala
        [Route("Rentals/Details/{id}")]
        public ActionResult Details(int id)
        {
            var rental = _context.Rentals.Include(r => r.Customer).SingleOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        // Metoda za brisanje rentala; dozvoljena samo ako je korisnik u ulozi "Administrator"
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult DeleteRental(int id)
        {
            var rentalToDelete = _context.Rentals.FirstOrDefault(c => c.Id == id);
            _context.Rentals.Remove(rentalToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index", "Rentals");
        }

        // Metoda za prikaz forme za izmenu podataka rentala; dozvoljena samo ako je korisnik u ulozi "Administrator"
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult Edit(int id)
        {
            var rental = _context.Rentals.Include(r => r.Bicycle).Include(r => r.Customer).SingleOrDefault(c => c.Id == id);

            if (rental == null)
            {
                return HttpNotFound();
            }
            var customers = _context.Customers.ToList();
            var bicycles = _context.Bicycles.ToList();
            var rentalViewModel = new RentalFormViewModel()
            {
                Customers = customers,
                Bicycles = bicycles,
                Rental = rental
            };
            return View("RentalForm", rentalViewModel);
        }

        // Metoda za ažuriranje podataka rentala; dozvoljena samo ako je korisnik u ulozi "Administrator"
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult UpdateReturnedRental(int id)
        {
            var rental = _context.Rentals.SingleOrDefault(c => c.Id == id);
            rental.DateReturned = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index", "Rentals");
        }
    }
}