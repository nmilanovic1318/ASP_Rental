using AutoMapper;
using RVAS_Bicikle.DataTransferObjects;
using RVAS_Bicikle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace RVAS_Bicikle.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        // Inicijalizacija DBContexta u konstruktoru
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // Prevencija curenja memorije (detaljnije objašnjeno u BicyclesController)
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Ruta/request: GET /api/customers
        public IHttpActionResult GetCustomers()
        {
            // Izvlače se sve mušterije, zajedno sa njihovim rentalima; ova lista se mapira na listu tipa customerDtos (CustomerDTO je isto što i Customer, samo odvojena klasa koja se koristi za rad sa API-jem iz sigurnosnih razloga); vraća se Ok REST response (200) i prosleđuje se lista CustomerDTO-a (mušterija)
            var customersWithRentals = _context.Customers.Include(c => c.Rentals);
            var customerDtos = customersWithRentals.ToList().Select(Mapper.Map<Customer, CustomerDto>);
            return Ok(customerDtos);
        }

        // Ruta/request: GET /api/customers/{id}
        public IHttpActionResult GetCustomer(int id)
        {
            // Izvlačenje tražene mušterije preko lambda expressiona
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                // Vraća not found HTTP response ako mušterija sa traženim ID-jem ne postoji
                return NotFound();
            }
            // Vraća se Ok (200) response, mapira se Customer na CustomerDto, ponovo iz sigurnosnih razloga
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        // Ruta/request: POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);

            // Vraća BadRequest response u slučaju neuspele validacije
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Ako je uspešno prošla validacija dodaje se mušterija u listu i promena se propagira dalje do baze
            _context.Customers.Add(customer);
            _context.SaveChanges();


            // Setuje se ID svojstvo CustomerDTO-a, pošto DTO neće automatski dobiti generisan ID od strane baze
            customerDto.Id = customer.Id;

            // Po RESTful principu vraća se kod 201 (Created), sa URI-jem koji vodi do nove mušterije
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

        // Ruta/request: PUT /api/customers/{id}
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            // Validacija
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingCustomer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Podešavanje podataka postojećeg korisnika preko mappera (compiler može da izvuče tipove preko prosleđenih varijabli)
            Mapper.Map(customerDto, existingCustomer);

            _context.SaveChanges();

            return Ok();
        }

        // Ruta/request: DELETE /api/customers/{id}
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {

            var existingCustomer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(existingCustomer);
            _context.SaveChanges();
            return Ok();
        }
    }
}
