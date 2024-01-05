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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<ProductCategoriesController> _logger;

        public ProductCategoriesController(AdventureWorksLt2019Context context, ILogger<ProductCategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            try
            {
                _logger.LogInformation("Getting all product categories");
                if (_context.ProductCategories == null)
                {
                    _logger.LogWarning("Product categories list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all product categories: {ex.Message}");
            }
         
            return await _context.ProductCategories.ToListAsync();
        }

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product category with ID: {id}");
                if (_context.ProductCategories == null)
                {
                    _logger.LogWarning("Product categories list is null");
                    return NotFound();
                }
                var productCategory = await _context.ProductCategories.FindAsync(id);

                if (productCategory == null)
                {
                    _logger.LogWarning($"Product category with ID {id} not found");
                    return NotFound();
                }

                return productCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting product category with ID {id}: {ex.Message}");
            }
          return NotFound();    
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
           
            try
            {
                _logger.LogInformation($"Updating product category with ID: {id}");
                if (id != productCategory.ProductCategoryId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(productCategory).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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
                _logger.LogError($"An error occurred while updating product category with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            try
            {
                _logger.LogInformation("Creating a new product category");
                if (_context.ProductCategories == null)
                {
                    _logger.LogWarning("Product categories list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.ProductCategories'  is null.");
                }
                _context.ProductCategories.Add(productCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while creating a new product category: {ex.Message}");
                if (ProductCategoryExists(productCategory.ProductCategoryId))
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
                _logger.LogError($"An error occurred while creating a new product category: {ex.Message}");
            }

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product category with ID: {id}");
                if (_context.ProductCategories == null)
                {
                    _logger.LogWarning("Product categories list is null");
                    return NotFound();
                }
                var productCategory = await _context.ProductCategories.FindAsync(id);
                if (productCategory == null)
                {
                    _logger.LogWarning($"Product category with ID {id} not found");
                    return NotFound();
                }

                _context.ProductCategories.Remove(productCategory);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting product category with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}
