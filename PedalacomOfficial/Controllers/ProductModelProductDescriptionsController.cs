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
    public class ProductModelProductDescriptionsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<ProductModelProductDescriptionsController> _logger;
        public ProductModelProductDescriptionsController(AdventureWorksLt2019Context context, ILogger<ProductModelProductDescriptionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductModelProductDescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModelProductDescription>>> GetProductModelProductDescriptions()
        {
            try
            {
                _logger.LogInformation("Getting all product model product descriptions");
                if (_context.ProductModelProductDescriptions == null)
                {
                    _logger.LogWarning("Product model product descriptions list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all product model product descriptions: {ex.Message}");
            }
          
            return await _context.ProductModelProductDescriptions.ToListAsync();
        }

        // GET: api/ProductModelProductDescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModelProductDescription>> GetProductModelProductDescription(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product model product description with ID: {id}");
                if (_context.ProductModelProductDescriptions == null)
                {
                    _logger.LogWarning("Product model product descriptions list is null");
                    return NotFound();
                }
                var productModelProductDescription = await _context.ProductModelProductDescriptions.FindAsync(id);

                if (productModelProductDescription == null)
                {
                    _logger.LogWarning($"Product model product description with ID {id} not found");
                    return NotFound();
                }

                return productModelProductDescription;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting product model product description with ID {id}: {ex.Message}");
            }
         return NotFound();
        }

        // PUT: api/ProductModelProductDescriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductModelProductDescription(int id, ProductModelProductDescription productModelProductDescription)
        {
            try
            {

                if (id != productModelProductDescription.ProductModelId)
                {
                    return BadRequest();
                }

                _context.Entry(productModelProductDescription).State = EntityState.Modified;

                if (id != productModelProductDescription.ProductModelId)
                {
                    return BadRequest();
                }

                _context.Entry(productModelProductDescription).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelProductDescriptionExists(id))
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

            }

            return NoContent();
        }

        // POST: api/ProductModelProductDescriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductModelProductDescription>> PostProductModelProductDescription(ProductModelProductDescription productModelProductDescription)
        {
            


            try
            {
                _logger.LogInformation("Creating a new product model product description");
                if (_context.ProductModelProductDescriptions == null)
                {
                    _logger.LogWarning("Product model product descriptions list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.ProductModelProductDescriptions'  is null.");
                }
                _context.ProductModelProductDescriptions.Add(productModelProductDescription);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while creating a new product model product description: {ex.Message}");
                if (ProductModelProductDescriptionExists(productModelProductDescription.ProductModelId))
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
                _logger.LogError($"An error occurred while creating a new product model product description: {ex.Message}");
            }

            return CreatedAtAction("GetProductModelProductDescription", new { id = productModelProductDescription.ProductModelId }, productModelProductDescription);
        }

        // DELETE: api/ProductModelProductDescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductModelProductDescription(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product model product description with ID: {id}");
                if (_context.ProductModelProductDescriptions == null)
                {
                    _logger.LogWarning("Product model product descriptions list is null");
                    return NotFound();
                }
                var productModelProductDescription = await _context.ProductModelProductDescriptions.FindAsync(id);
                if (productModelProductDescription == null)
                {
                    _logger.LogWarning($"Product model product description with ID {id} not found");
                    return NotFound();
                }

                _context.ProductModelProductDescriptions.Remove(productModelProductDescription);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting product model product description with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool ProductModelProductDescriptionExists(int id)
        {
            return (_context.ProductModelProductDescriptions?.Any(e => e.ProductModelId == id)).GetValueOrDefault();
        }
    }
}
