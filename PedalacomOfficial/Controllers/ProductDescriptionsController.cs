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
    public class ProductDescriptionsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<ProductDescriptionsController> _logger;
        public ProductDescriptionsController(AdventureWorksLt2019Context context, ILogger<ProductDescriptionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductDescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDescription>>> GetProductDescriptions()
        {
            try
            {
                _logger.LogInformation("Getting all product descriptions");
                if (_context.ProductDescriptions == null)
                {
                    _logger.LogWarning("Product descriptions list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all product descriptions: {ex.Message}");
            }
            return await _context.ProductDescriptions.ToListAsync();
        }

        // GET: api/ProductDescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDescription>> GetProductDescription(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product description with ID: {id}");
                if (_context.ProductDescriptions == null)
                {
                    _logger.LogWarning("Product descriptions list is null");
                    return NotFound();
                }
                var productDescription = await _context.ProductDescriptions.FindAsync(id);

                if (productDescription == null)
                {
                    _logger.LogWarning($"Product description with ID {id} not found");
                    return NotFound();
                }

                return productDescription;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting product description with ID {id}: {ex.Message}");
            }
          return NotFound();
        }

        // PUT: api/ProductDescriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDescription(int id, ProductDescription productDescription)
        {
            try
            {
                _logger.LogInformation($"Updating product description with ID: {id}");
                if (id != productDescription.ProductDescriptionId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(productDescription).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDescriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occurred while updating product description with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/ProductDescriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDescription>> PostProductDescription(ProductDescription productDescription)
        {
            try
            {
                _logger.LogInformation("Creating a new product description");

                // Genera un nuovo Rowguid 
                productDescription.Rowguid = Guid.NewGuid();

                if (_context.ProductDescriptions == null)
                {
                    _logger.LogWarning("Product descriptions list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.ProductDescriptions'  is null.");
                }

                _context.ProductDescriptions.Add(productDescription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while creating a new product description: {ex.Message}");
                if (ProductDescriptionExists(productDescription.ProductDescriptionId))
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
                _logger.LogError($"An error occurred while creating a new product description: {ex.Message}");
            }

            return CreatedAtAction("GetProductDescription", new { id = productDescription.ProductDescriptionId }, productDescription);
        }


        // DELETE: api/ProductDescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDescription(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product description with ID: {id}");
                if (_context.ProductDescriptions == null)
                {
                    _logger.LogWarning("Product descriptions list is null");
                    return NotFound();
                }
                var productDescription = await _context.ProductDescriptions.FindAsync(id);
                if (productDescription == null)
                {
                    _logger.LogWarning($"Product description with ID {id} not found");
                    return NotFound();
                }

                _context.ProductDescriptions.Remove(productDescription);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting product description with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool ProductDescriptionExists(int id)
        {
            return (_context.ProductDescriptions?.Any(e => e.ProductDescriptionId == id)).GetValueOrDefault();
        }
    }
}
