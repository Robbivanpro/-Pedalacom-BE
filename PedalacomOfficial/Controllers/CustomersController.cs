using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;

namespace PedalacomOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public CustomersController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            var existingCostumer = _context.Customers.FirstOrDefault(x => x.CustomerId == id);

            if (existingCostumer == null)
            {
                return NotFound();
            }

            existingCostumer.Title = customer.Title;
            existingCostumer.FirstName = customer.FirstName;
            existingCostumer.MiddleName = customer.MiddleName;
            existingCostumer.LastName = customer.LastName;
            existingCostumer.Suffix = customer.Suffix;
            existingCostumer.CompanyName = customer.CompanyName;
            existingCostumer.SalesPerson = customer.SalesPerson;
            existingCostumer.EmailAddress = customer.EmailAddress;
            existingCostumer.PasswordHash = customer.PasswordHash;
            existingCostumer.PasswordSalt = customer.PasswordSalt;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }



            return NoContent();

        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // Imposta il Rowguid con un nuovo GUID
            customer.Rowguid = Guid.NewGuid();

            // Imposta la data di modifica corrente
            customer.ModifiedDate = DateTime.UtcNow;

            // Aggiungi il nuovo cliente al contesto
            _context.Customers.Add(customer);

            try
            {
                // Salva le modifiche nel contesto (cioè nel database)
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Gestisci eventuali eccezioni specifiche che potrebbero verificarsi durante il salvataggio
                return StatusCode(500, "An error occurred while saving the customer");
            }

            // Restituisci un risultato di creazione con l'oggetto Customer e l'URL per accedervi
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }


        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
