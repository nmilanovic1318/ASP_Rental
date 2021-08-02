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
    [Authorize(Roles = RoleName.Administrator)]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        // instanciranje DBContexta kroz konstruktor
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // prevencija curenja memorije
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers

        public ActionResult Index()
        {
            // Izvlačenje svih mušterija u listu, vraćanje Index View-a sa prosleđenim mušterijama
            var customers = _context.Customers.ToList();
            return View(customers);
        }


        // Metoda za prikaz forme za stvaranje nove mušterije
        [HttpGet]
        public ActionResult Create()
        {
            Customer customer = new Customer();
            return View("CustomerForm", customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(Customer customer)
        {
            // Provera da li je sve u redu sa modelom; u slučaju da nešto nije u redu, vraća View popunjen onako kako je bio u trenutku exceptiona
            if (!ModelState.IsValid)
            {
                var modelCustomer = customer;
                return View("CustomerForm", modelCustomer);
            }
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var ExistingCustomer = _context.Customers.SingleOrDefault(c => c.Id == customer.Id);
                ExistingCustomer.Name = customer.Name;
                ExistingCustomer.Surname = customer.Surname;
                ExistingCustomer.Age = customer.Age;
                ExistingCustomer.PhoneNumber = customer.PhoneNumber;
                ExistingCustomer.Address = customer.Address;

            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        // Metoda za prikaz detalja mušterije
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(r => r.Rentals).SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // Metoda za brisanje mušterije iz baze
        public ActionResult DeleteCustomer(int id)
        {
            var customerToDelete = _context.Customers.FirstOrDefault(c => c.Id == id);
            _context.Customers.Remove(customerToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }

        // Metoda za prikaz forme za izmenu podataka mušterije
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("CustomerForm", customer);
        }
    }
}