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
        private readonly ILogger<CustomersController> _logger;
        public CustomersController(AdventureWorksLt2019Context context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                _logger.LogInformation("Getting all customers");
                if (_context.Customers == null)
                {
                    _logger.LogWarning("Customers list is null");
                    return NotFound();
                }
               
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all customers: {ex.Message}");
            }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                _logger.LogInformation($"Getting customer with ID: {id}");
                if (_context.Customers == null)
                {
                    _logger.LogWarning("Customers list is null");
                    return NotFound();
                }
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found");
                    return NotFound();
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting customer with ID {id}: {ex.Message}");
            }
          return NotFound();
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            try
            {
                _logger.LogInformation($"Updating customer with ID: {id}");
                if (id != customer.CustomerId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Concurrency exception while updating customer with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                _logger.LogInformation("Creating a new customer");
                if (_context.Customers == null)
                {
                    _logger.LogWarning("Customers list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.Customers'  is null.");
                }
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while creating a new customer: {ex.Message}");
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a new customer: {ex.Message}");
            }
          
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer with ID: {id}");
                if (_context.Customers == null)
                {
                    _logger.LogWarning("Customers list is null");
                    return NotFound();
                }
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found");
                    return NotFound();
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting customer with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
