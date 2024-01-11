using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;
using PedalacomOfficial.Models.DTO;

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
        public IActionResult UpdateCustomerAddress([FromBody] CustomerAddressDTO custumerAddressDTO)
        {
            if (custumerAddressDTO == null)
            {
                _logger.LogError("UpdateCustomerAddress chiamato con DTO null.");
                return BadRequest("Il DTO non può essere null.");
                
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == custumerAddressDTO.CustomerId);
                    var address = _context.Addresses.FirstOrDefault(a => a.AddressId == custumerAddressDTO.AddressId);

                    _logger.LogInformation("Inizio aggiornamento CustomerAddress");

                    if (customer == null)
                    {
                        customer = new Customer { CustomerId = custumerAddressDTO.CustomerId, FirstName = custumerAddressDTO.FirstName, Suffix = custumerAddressDTO.Suffix, MiddleName = custumerAddressDTO.MiddleName, 
                            LastName = custumerAddressDTO.LastName, EmailAddress = custumerAddressDTO.EmailAddress, Phone = custumerAddressDTO.Phone };
                        _context.Customers.Add(customer);
                    }

                    //Creazione del cliente
                    else
                    {
                        // Aggiorna cliente
                        customer.Title = custumerAddressDTO.Title;
                        customer.FirstName = custumerAddressDTO.FirstName;
                        customer.Suffix = custumerAddressDTO.Suffix;
                        customer.MiddleName = custumerAddressDTO.MiddleName;
                        customer.LastName = custumerAddressDTO.LastName;
                        customer.EmailAddress = custumerAddressDTO.EmailAddress;
                        customer.Phone = custumerAddressDTO.Phone;
                    }
                    

                    //Creazione dell'indirizzo
                    if (address == null)
                    {
                        address = new Address
                        {
                            AddressId = custumerAddressDTO.AddressId,
                            City = custumerAddressDTO.City,
                            AddressLine1 = custumerAddressDTO.AddressLine1,
                            AddressLine2 = custumerAddressDTO.AddressLine2,
                            CountryRegion = custumerAddressDTO.CountryRegion,
                            StateProvince = custumerAddressDTO.StateProvince,
                            PostalCode = custumerAddressDTO.PostalCode
                        };
                        _context.Addresses.Add(address);
                        

                        
                    }
                    else
                    {
                        // Aggiorna indirizzo
                        address.City = custumerAddressDTO.City;
                        address.AddressLine1 = custumerAddressDTO.AddressLine1;
                        address.AddressLine2 = custumerAddressDTO.AddressLine2;
                        address.StateProvince = custumerAddressDTO.StateProvince;
                        address.CountryRegion = custumerAddressDTO.CountryRegion;
                        address.PostalCode = custumerAddressDTO.PostalCode;

                    }                  

                    _context.SaveChanges();

                    var customerAddress = _context.CustomerAddresses.FirstOrDefault(ca => ca.CustomerId == customer.CustomerId && ca.AddressId == address.AddressId);

                    if (customerAddress == null )
                    {
                        customerAddress = new CustomerAddress 
                        { 
                            CustomerId = customer.CustomerId, 
                            AddressId = address.AddressId, 
                            ModifiedDate = DateTime.Now, 
                            AddressType = customerAddress.AddressType
                        
                        };

                        _context.CustomerAddresses.Add(customerAddress);
                    }                    
                    else
                    {


                        customerAddress.AddressType = customerAddress.AddressType;
                        customerAddress.ModifiedDate = DateTime.Now;

;
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    _logger.LogInformation("CustomerAddress aggiornato con successo");

                    return Ok("Record aggiornati con successo");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento di CustomerAddress: " + ex.Message);
                    transaction.Rollback();
                    return BadRequest($"Errore durante l'aggiornamento: {ex.Message}");
                }
            }
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
