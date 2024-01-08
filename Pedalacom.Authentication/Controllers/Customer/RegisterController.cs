using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using library.Security;
using PedalacomOfficial.Models;


namespace Pedalacom.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<RegisterController> _logger;
        private readonly Cryptography _cryptoLibrary;

        public RegisterController(AdventureWorksLt2019Context context, ILogger<RegisterController> logger, Cryptography cryptoLibrary)
        {
            _context = context;
            _logger = logger;
            _cryptoLibrary = cryptoLibrary;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PedalacomOfficial.Models.Customer>> Register(PedalacomOfficial.Models.Customer customer)
        {
            try
            {
                if (_context.Customers.Any(c => c.EmailAddress == customer.EmailAddress))
                {
                    return Conflict("User with the specified email already exists.");
                }

                var cryptoResult = _cryptoLibrary.EncrypSaltString(_context, customer.EmailAddress, customer.PasswordHash);
                customer.PasswordHash = cryptoResult.Key;
                customer.PasswordSalt = cryptoResult.Value;

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while registering a new user: {ex.Message}");
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
                _logger.LogError($"An error occurred while registering a new user: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
