using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
        {
            try
            {
                _logger.LogInformation("Getting all product categories");
                if (_context.ProductCategories == null)
                {
                    _logger.LogWarning("Product categories list is null");
                    return NotFound();
                }

                var categories = await _context.ProductCategories.ToListAsync();

                // Mappatura manuale da ProductCategory a ProductCategoryDto
                var categoryDtos = categories.Select(c => new ProductCategoryDTO
                {
                    ProductCategoryId = c.ProductCategoryId,
                    ParentProductCategoryId = c.ParentProductCategoryId,
                    Name = c.Name,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

                return categoryDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all product categories: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
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
        public async Task<IActionResult> PutProductCategory(int id, ProductCategoryUpdate productCategoryUpdate)
        {
            if (id != productCategoryUpdate.ProductCategoryId)
            {
                _logger.LogError($"Bad request - ID mismatch. Path ID: {id}, DTO ID: {productCategoryUpdate.ProductCategoryId}");
                return BadRequest("ID mismatch");
            }

            var existingProductCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.ProductCategoryId == id);

            if (existingProductCategory == null)
            {
                _logger.LogWarning($"ProductCategory with ID {id} not found");
                return NotFound();
            }

            existingProductCategory.Name = productCategoryUpdate.Name;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductCategoryExists(id))
                {
                    _logger.LogWarning($"ProductCategory with ID {id} not found during DbUpdateConcurrencyException. Exception: {ex.Message}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"DbUpdateConcurrencyException while updating ProductCategory with ID {id}: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating product category with ID {id}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }


        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategoryDTO>> PostProductCategory(ProductCategoryDTO productCategoryDTO)
        {
            _logger.LogInformation("Inizio processo di inserimento di una nuova ProductCategory");

            if (productCategoryDTO == null)
            {
                _logger.LogError("PostProductCategory chiamato con un DTO null");
                return BadRequest("Il DTO non può essere null.");
            }


            _logger.LogInformation($"Generato Rowguid: {productCategoryDTO.Rowguid} per il nuovo ProductCategory");

            // Mappatura del DTO all'entità ProductCategory
            var productCategory = new ProductCategory
            {
                Name = productCategoryDTO.Name,
                ModifiedDate = productCategoryDTO.ModifiedDate,
                Rowguid = Guid.NewGuid()

        };

            _context.ProductCategories.Add(productCategory);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("ProductCategory salvato con successo nel database");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il salvataggio di ProductCategory nel database");
                return BadRequest("Errore durante il salvataggio nel database");
            }

            // Aggiornare il DTO con l'ID generato dal database
            productCategoryDTO.ProductCategoryId = productCategory.ProductCategoryId;

            _logger.LogInformation($"ProductCategory creato con ID: {productCategory.ProductCategoryId}");

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategoryDTO);
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
