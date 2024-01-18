using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;
using PedalacomOfficial.Models.DTO;

namespace PedalacomOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(AdventureWorksLt2019Context context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products");
                if (_context.Products == null)
                {
                    _logger.LogWarning("Products list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all products: {ex.Message}");
            }

            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product with ID: {id}");
                if (_context.Products == null)
                {
                    _logger.LogWarning("Products list is null");
                    return NotFound();
                }
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found");
                    return NotFound();
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting product with ID {id}: {ex.Message}");
            }
            return NotFound();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            try
            {
                _logger.LogInformation($"Updating product with ID: {id}");
                if (id != productDTO.ProductId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(productDTO).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Concurrency exception while updating product with ID {id}: {ex.Message}");
                if (!ProductExists(id))
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
                _logger.LogError($"An error occurred while updating product with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] ProductDTO productDTO)
        {
            _logger.LogInformation($"Received request: {JsonConvert.SerializeObject(productDTO)}");

            if (productDTO == null)
            {
                _logger.LogError("PostProduct chiamato con un DTO null.");
                return BadRequest("Il DTO non può essere null.");
            }

            // Esempio di validazione per il campo 'Name'
            int maxNameLength = 50; // Sostituisci con il limite massimo della tua colonna nel database
            if (productDTO.Name != null && productDTO.Name.Length > maxNameLength)
            {
                _logger.LogError($"La lunghezza del campo 'Name' supera il limite di {maxNameLength} caratteri.");
                return BadRequest($"La lunghezza del campo 'Name' non può superare {maxNameLength} caratteri.");
            }

            // Creazione di Product
            var product = new Product
            {
                Name = productDTO.Name,
                ProductNumber = productDTO.ProductNumber,
                Color = productDTO.Color,
                StandardCost = productDTO.StandardCost,
                ListPrice = productDTO.ListPrice,
                Size = productDTO.Size,
                Weight = productDTO.Weight,
                ThumbnailPhotoFileName = productDTO.ThumbnailPhotoFileName,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow,
                SellStartDate = DateTime.UtcNow,
                SellEndDate = null,
                DiscontinuedDate = null
            };

            if (!string.IsNullOrEmpty(productDTO.ThumbNailPhotoBase64))
            {
                try
                {
                    product.ThumbNailPhoto = Convert.FromBase64String(productDTO.ThumbNailPhotoBase64);
                }
                catch (FormatException ex)
                {
                    _logger.LogError($"Errore nella conversione dell'immagine thumbnail: {ex.Message}");
                    return BadRequest("Formato dell'immagine thumbnail non valido.");
                }
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Errore di aggiornamento del database: {ex.Message}");
                return StatusCode(500, "Errore interno del server.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore generico: {ex.Message}");
                return StatusCode(500, "Errore interno del server.");
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }
   


    // DELETE: api/Products/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID: {id}");
                if (_context.Products == null)
                {
                    _logger.LogWarning("Products list is null");
                    return NotFound();
                }
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found");
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting product with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
