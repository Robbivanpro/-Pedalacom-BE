using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;

namespace PedalacomOfficial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    

    public class ProductModelsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        private readonly ILogger<ProductModelsController> _logger;
        public ProductModelsController(AdventureWorksLt2019Context context, ILogger<ProductModelsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/ProductModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductModels()
        {
          if (_context.ProductModels == null)
          {
              return NotFound();
          }
            return await _context.ProductModels.ToListAsync();
        }

        // GET: api/ProductModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductModel(int id)
        {
          if (_context.ProductModels == null)
          {
              return NotFound();
          }
            var productModel = await _context.ProductModels.FindAsync(id);

            if (productModel == null)
            {
                return NotFound();
            }

            return productModel;
        }

        // PUT: api/ProductModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductModel(int id, ProductModel productModel)
        {
            _logger.LogInformation("Tentativo di aggiornamento del modello di prodotto con ID: {ProductId}", id);

            if (!ProductModelExists(id))
            {
                _logger.LogWarning("Modello di prodotto non trovato con ID: {ProductId}", id);
                return NotFound();
            }

            var existingProduct = _context.ProductModels.FirstOrDefault(p => p.ProductModelId == id);
            if (existingProduct == null)
            {
                _logger.LogWarning("Modello di prodotto non trovato nel database con ID: {ProductId}", id);
                return NotFound();
            }

            // Aggiorna qui gli attributi, escludendo il rowguid
            existingProduct.Name = productModel.Name;
            existingProduct.CatalogDescription = $@"
            <?xml-stylesheet href='ProductDescription.xsl' type='text/xsl'?>
             <p1:ProductDescription xmlns:p1='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelDescription'
                   xmlns:wm='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelWarrAndMain'
                   xmlns:wf='http://www.adventure-works.com/schemas/OtherFeatures'
                   xmlns:html='http://www.w3.org/1999/xhtml'
                   ProductModelID='{existingProduct.ProductModelId}'
                   ProductModelName='{System.Security.SecurityElement.Escape(existingProduct.Name)}'>
                   <p1:Summary>
                   <html:p>{System.Security.SecurityElement.Escape(productModel.CatalogDescription)}</html:p>
                   </p1:Summary>
                   <!-- Aggiungi altri elementi XML conformi allo schema qui -->
                   </p1:ProductDescription>";


            _context.ProductModels.Update(existingProduct);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Prodotto aggiornato con successo con ID: {ProductId}", id);
            return NoContent();
        }

        private bool ProductModelExists(int id)
        {
            var exists = _context.ProductModels.Any(e => e.ProductModelId == id);
            _logger.LogInformation("Verifica esistenza del modello di prodotto con ID: {ProductId}: {Exists}", id, exists);
            return exists;
        }




        // POST: api/ProductModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProductModel(ProductModel productModel)
        {
            if (_context.ProductModels == null)
            {
                return Problem("Entity set '_context.ProductModels' is null.");
            }

            productModel.Rowguid = Guid.NewGuid();

            // Assicurati che CatalogDescription sia formattato secondo lo schema richiesto
            productModel.CatalogDescription = $@"
            <?xml-stylesheet href='ProductDescription.xsl' type='text/xsl'?>
             <p1:ProductDescription xmlns:p1='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelDescription'
                           xmlns:wm='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelWarrAndMain'
                           xmlns:wf='http://www.adventure-works.com/schemas/OtherFeatures'
                           xmlns:html='http://www.w3.org/1999/xhtml'
                           ProductModelID='{productModel.ProductModelId}'
                           ProductModelName='{System.Security.SecurityElement.Escape(productModel.Name)}'>
             <p1:Summary>
             <html:p>{System.Security.SecurityElement.Escape(productModel.CatalogDescription)}</html:p>
             </p1:Summary>
             <!-- Aggiungi altri elementi XML conformi allo schema qui -->
             </p1:ProductDescription>";

            _context.ProductModels.Add(productModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "A database error occurred: " + ex.InnerException?.Message);
            }

            return CreatedAtAction("GetProductModel", new { id = productModel.ProductModelId }, productModel);
        }


        // DELETE: api/ProductModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductModel(int id)
        {
            if (_context.ProductModels == null)
            {
                return NotFound();
            }
            var productModel = await _context.ProductModels.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            _context.ProductModels.Remove(productModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
