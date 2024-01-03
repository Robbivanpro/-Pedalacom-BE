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
    public class AddressesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<AddressesController> _logger;
        public AddressesController(AdventureWorksLt2019Context context, ILogger<AddressesController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into AddressController");
        }
        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            try
            {
                _logger.LogInformation("Getting addresses from the database");

                if (_context.Addresses == null)
                {
                    _logger.LogWarning("Address list is null");
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting addresses: {ex.Message}");
            }
            return await _context.Addresses.ToListAsync();
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            try
            {
                _logger.LogInformation($"Getting address with ID: {id}");
                if (_context.Addresses == null)
                {
                    _logger.LogWarning("Address list is null");
                    return NotFound();
                }
                var address = await _context.Addresses.FindAsync(id);

                if (address == null)
                {
                    _logger.LogWarning($"Address with ID {id} not found");
                    return NotFound();
                }

                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting address with ID {id}: {ex.Message}");
            }
            return NotFound();
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            try
            {
                _logger.LogInformation($"Updating address with ID: {id}");
                if (id != address.AddressId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                var existingAddress = _context.Addresses.FirstOrDefault(x => x.AddressId == id);

                if (existingAddress == null)
                {
                    _logger.LogWarning($"Address with ID {id} not found");
                    return NotFound();
                }

                existingAddress.AddressLine1 = address.AddressLine1;
                existingAddress.AddressLine2 = address.AddressLine2;
                existingAddress.City = address.City;
                existingAddress.StateProvince = address.StateProvince;
                existingAddress.CountryRegion = address.CountryRegion;
                existingAddress.PostalCode = address.PostalCode;
                await _context.SaveChangesAsync();
            }


            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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
                _logger.LogError($"An error occurred while updating address with ID {id}: {ex.Message}");
            }

              return NoContent();
        }

           

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            try
            {
                _logger.LogInformation("Creating a new address");

                if (_context.Addresses == null)
                {
                    _logger.LogError("Entity set 'AdventureWorksLt2019Context.Addresses' is null.");
                    return Problem("Entity set 'AdventureWorksLt2019Context.Addresses'  is null.");
                }
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a new address: {ex.Message}");
            }
            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }


        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting address with ID: {id}");
                if (_context.Addresses == null)
                {
                    _logger.LogWarning("Address list is null");
                    return NotFound();
                }
                var address = await _context.Addresses.FindAsync(id);
                if (address == null)
                {
                    return NotFound();
                }

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting address with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
