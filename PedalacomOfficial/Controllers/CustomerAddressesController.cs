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
    public class CustomerAddressesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<CustomerAddressesController> _logger;
        public CustomerAddressesController(AdventureWorksLt2019Context context, ILogger<CustomerAddressesController> logger)
        {
            _context = context;
            _logger = logger;  // Inizializzato il logger
        }

        // GET: api/CustomerAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetCustomerAddresses()
        {
            try
            {
                _logger.LogInformation("Getting all customer addresses");
                if (_context.CustomerAddresses == null)
                {
                    _logger.LogWarning("CustomerAddresses list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all customer addresses: {ex.Message}");
            }
          
            return await _context.CustomerAddresses.ToListAsync();
        }

        // GET: api/CustomerAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAddress>> GetCustomerAddress(int id)
        {
            try
            {
                _logger.LogInformation($"Getting customer address with ID: {id}");
                if (_context.CustomerAddresses == null)
                {
                    _logger.LogWarning("CustomerAddresses list is null");
                    return NotFound();
                }
                var customerAddress = await _context.CustomerAddresses.FindAsync(id);

                if (customerAddress == null)
                {
                    _logger.LogWarning($"Customer address with ID {id} not found");
                    return NotFound();
                }

                return customerAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting customer address with ID {id}: {ex.Message}");
            }
          return NotFound();
        }

        // PUT: api/CustomerAddresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAddress(int id, CustomerAddress customerAddress)
        {
            try
            {
                _logger.LogInformation($"Updating customer address with ID: {id}");
                if (id != customerAddress.CustomerId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(customerAddress).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAddressExists(id))
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
                _logger.LogError($"Concurrency exception while updating customer address with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/CustomerAddresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerAddress>> PostCustomerAddress(CustomerAddress customerAddress)
        {
            try
            {
                _logger.LogInformation("Creating a new customer address");
                if (_context.CustomerAddresses == null)
                {
                    _logger.LogWarning("CustomerAddresses list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.CustomerAddresses'  is null.");
                }
                _context.CustomerAddresses.Add(customerAddress);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerAddressExists(customerAddress.CustomerId))
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
                _logger.LogError($"An error occurred while creating a new customer address: {ex.Message}");
            }
            
            return CreatedAtAction("GetCustomerAddress", new { id = customerAddress.CustomerId }, customerAddress);
        }

        // DELETE: api/CustomerAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAddress(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer address with ID: {id}");
                if (_context.CustomerAddresses == null)
                {
                    _logger.LogWarning("CustomerAddresses list is null");
                    return NotFound();
                }
                var customerAddress = await _context.CustomerAddresses.FindAsync(id);
                if (customerAddress == null)
                {
                    _logger.LogWarning($"Customer address with ID {id} not found");
                    return NotFound();
                }

                _context.CustomerAddresses.Remove(customerAddress);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting customer address with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool CustomerAddressExists(int id)
        {
            return (_context.CustomerAddresses?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
