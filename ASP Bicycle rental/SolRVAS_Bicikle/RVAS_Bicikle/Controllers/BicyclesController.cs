using RVAS_Bicikle.Models;
using RVAS_Bicikle.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace RVAS_Bicikle.Controllers
{
    public class BicyclesController : Controller
    {

        private ApplicationDbContext _context;

        // instanciranje DBContexta kroz konstruktor
        public BicyclesController()
        {
            _context = new ApplicationDbContext();
        }

        // Prevencija curenja memorije
        // Detaljnije: Za Dispose metodu je zaduzen GC (Garbage Collector); on ne moze da dohvati konekciju sa bazom,
        // jer ona ne prolazi kroz CLR; zbog toga rucno kucamo disposing kako bi ocistili ostatke iz memorije ručno
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Bicycles
        [AllowAnonymous] // dozvoljeno neulogovanim korisnicima da čitaju objekte klase Bicycle
        public ActionResult Index(string searchBy, string search, string orderBy, int? page)
        {
            // Ukoliko korisnik nije u ulozi "User" ili "Administrator" dobiće vraćen view "ReadOnlyBycicleList", sa mogućnošću pretrage i sortiranja
            if (!User.IsInRole(RoleName.User) && !User.IsInRole(RoleName.Administrator))
            {

                // Krećemo sa pretpostavkom da treba da prikažemo sve bicikle korisniku
                var bicycles = _context.Bicycles.Include(b => b.Brand);

                // Proveravamo da li je search termin prazan; ako jeste, prikazaćemo sve bicikle korisniku jer ništa konkretno nije pretražio; ako nije, nameštamo kolekciju koju prikazujemo u odnosu na traženi search termin

                if (!String.IsNullOrEmpty(search))
                {
                    // Ako je search kriterijum "sve", prikazujemo sve bicikle čiji se bilo koji element poklapa sa search terminom
                    if (searchBy == "All")
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Model.Contains(search) || b.Frame.Contains(search) || b.Price.ToString().Contains(search) || b.Weight.ToString().Contains(search) || b.Brand.Name.Contains(search));
                    }

                    // Ako je search kriterijum model, prikazujemo bicikle čiji se model poklapa sa search terminom
                    else if (searchBy == "Model")
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Model.Contains(search) || search == null);
                    }

                    // U suprotnom, znamo da je search termin brand; prikazujemo sve bicikle gde se naziv brenda poklapa sa search terminom
                    else
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Brand.Name.Contains(search) || search == null);
                    }

                }

                // Sortiranje (kolekciju koju smo prosledili iz koda iznad sortiramo prema onome što je izabrano na front endu
                switch (orderBy)
                {
                    case "Model":
                        bicycles = bicycles.OrderBy(b => b.Model);
                        break;
                    case "Brand":
                        bicycles = bicycles.OrderBy(b => b.Brand.Name);
                        break;
                    default:
                        bicycles = bicycles.OrderBy(b => b.Price);
                        break;
                }
                return View("ReadOnlyBicycleList", bicycles.ToPagedList(page ?? 1, 3));

            }
            // Ako je korisnik u ulozi "User" ili "Administrator", korisnik dobija vraćen View Index, gde može da vrši CRUD operacije, kao i pretragu / sortiranje
            else
            {

                // Ovo je sve isto kao iznad
                var bicycles = _context.Bicycles.Include(b => b.Brand);

                if (!String.IsNullOrEmpty(search))
                {
                    if (searchBy == "All")
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Model.Contains(search) || b.Frame.Contains(search) || b.Price.ToString().Contains(search) || b.Weight.ToString().Contains(search) || b.Brand.Name.Contains(search));
                    }
                    else if (searchBy == "Model")
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Model.Contains(search) || search == null);
                    }
                    else
                    {
                        bicycles = _context.Bicycles.Include(b => b.Brand).Where(b => b.Brand.Name.Contains(search) || search == null);
                    }
                }

                switch (orderBy)
                {
                    case "Model":
                        bicycles = bicycles.OrderBy(b => b.Model);
                        break;
                    case "Brand":
                        bicycles = bicycles.OrderBy(b => b.Brand.Name);
                        break;
                    default:
                        bicycles = bicycles.OrderBy(b => b.Price);
                        break;
                }
                return View(bicycles.ToPagedList(page ?? 1, 3));
            }

        }

        // Ruta za prikaz detalja pojedinačne bicikle
        [Route("Bicycles/Details/{id}")]
        public ActionResult Details(int id)
        {
            // Izvlači se tražena bicikla preko lambda expressiona, prosleđuje se na model radi prikaza podataka; ako nije nađena bicikla, vraća se HttpNotFound stranica
            var bicycle = _context.Bicycles.Include(b => b.Brand).SingleOrDefault(b => b.Id == id);
            if (bicycle == null)
            {
                return HttpNotFound();
            }
            return View(bicycle);
        }

        // vraća stranicu na kojoj je prikazana forma za ubacivanje nove bicikle
        [HttpGet]
        [Route("Bicycles/Create")]
        public ActionResult Create()
        {

            var brands = _context.Brands.ToList();

            var bicycleViewModel = new BicycleFormViewModel()
            {
                Bicycle = new Bicycle(),
                Brands = brands
            };
            return View("BicycleForm", bicycleViewModel);
        }


        // POST request na ovu rutu poziva metodu koja čuva podatke u DB
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Store(Bicycle bicycle)
        {
            // Provera da li je sve u redu sa modelom; u slučaju da nešto nije u redu, vraća View popunjen onako kako je bio u trenutku exceptiona (Serverska validacija)
            if (!ModelState.IsValid)
            {
                var brands = _context.Brands.ToList();
                var bicycleViewModel = new BicycleFormViewModel
                {
                    Bicycle = bicycle,
                    Brands = brands
                };
                return View("BicycleForm", bicycleViewModel);
            }
            // Ako je ID objekta jednak nuli, on ne postoji u bazi, mora da se doda
            if (bicycle.Id == 0)
            {
                _context.Bicycles.Add(bicycle);
            }
            // Ako ID objekta nije jednak nuli znači da postoji, nalazi se preko lambda expressiona i radi se update
            else
            {
                var ExistingBycicle = _context.Bicycles.SingleOrDefault(b => b.Id == bicycle.Id);
                ExistingBycicle.Frame = bicycle.Frame;
                ExistingBycicle.Brand = _context.Brands.SingleOrDefault(b => b.Id == bicycle.BrandId);
                ExistingBycicle.Price = bicycle.Price;
                ExistingBycicle.Weight = bicycle.Weight;
                ExistingBycicle.Model = bicycle.Model;
                ExistingBycicle.TrainingWheels = bicycle.TrainingWheels;
            }

            // U oba slučaja lokalne promene se propagiraju do baze na kraju izvršavanja
            _context.SaveChanges();

            // Vraća se redirekcija do index stranice na kojoj su prikazane sve bicikle
            return RedirectToAction("Index", "Bicycles");
        }

        // Metoda za prikaz forme za izmenu podataka 
        [HttpGet]
        [Route("Bicycles/Edit")]
        public ActionResult Edit(int id)
        {
            // Nalazi se tražena bicikla preko lambda expressiona; ako nije nađena, vraća se HttpNotFound stranica; u suprotnom, pravi se ViewModel koji prikazuje i bicikle i brendove i prosleđuje se na View (viewModel korišćen jer se koriste podaci iz dve različite tabele, da bi se za biciklu brend birao preko dropdown menija)
            var bicycle = _context.Bicycles.SingleOrDefault(b => b.Id == id);

            if (bicycle == null)
            {
                return HttpNotFound();
            }

            var bicycleViewModel = new BicycleFormViewModel()
            {
                Bicycle = bicycle,
                Brands = _context.Brands.ToList()

            };
            return View("BicycleForm", bicycleViewModel);
        }

        // Metoda za brisanje bicikle
        public ActionResult DeleteBicycle(int id)
        {
            // Nalazi se tražena bicikla preko lambda expressiona; briše se iz kolekcije, i izmena se propagira do baze

            var bicycleToDelete = _context.Bicycles.FirstOrDefault(b => b.Id == id);
            _context.Bicycles.Remove(bicycleToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index", "Bicycles");
        }

    }
}