using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
    public class ProductModelProductDescriptionsController : ControllerBase
    {
        private const string V = "Inner Exception: ";
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
        [HttpGet("{productModelId:int}/{productDescriptionId:int}")]
        public async Task<ActionResult<ProductModelDescriptionDTO>> GetProductModelDescription(int productModelId, int productDescriptionId)
        {
            // Cerca il ProductModel utilizzando productModelId
            var productModel = await _context.ProductModels.FindAsync(productModelId);
            if (productModel == null)
            {
                return NotFound($"ProductModel con ID {productModelId} non trovato.");
            }

            // Cerca il ProductDescription utilizzando productDescriptionId
            var productDescription = await _context.ProductDescriptions.FindAsync(productDescriptionId);
            if (productDescription == null)
            {
                return NotFound($"ProductDescription con ID {productDescriptionId} non trovato.");
            }

            // Crea il DTO combinando informazioni da productModel e productDescription
            var productModelDescriptionDTO = new ProductModelDescriptionDTO
            {
                ProductModelId = productModel.ProductModelId,
                Name = productModel.Name,
                CatalogDescription = productModel.CatalogDescription, // Assumi che sia il campo corretto
                Rowguid = productModel.Rowguid,
                ProductDescriptionId = productDescription.ProductDescriptionId,
                Description = productDescription.Description,
                ModifiedDate = productDescription.ModifiedDate
            };

            return Ok(productModelDescriptionDTO);
        }


        // PUT: api/ProductModelProductDescriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productModelId:int}/{productDescriptionId:int}")]
        public async Task<IActionResult> UpdateProductModelDescription(int productModelId, int productDescriptionId, [FromBody] ProductModelDescriptionDTO productModelDescriptionDTO)
        {
            if (productModelDescriptionDTO == null)
            {
                return BadRequest("Il DTO non può essere null.");
            }

            var productModel = await _context.ProductModels.FindAsync(productModelId);
            if (productModel == null)
            {
                return NotFound($"ProductModel con ID {productModelId} non trovato.");
            }

            var productDescription = await _context.ProductDescriptions.FindAsync(productDescriptionId);
            if (productDescription == null)
            {
                return NotFound($"ProductDescription con ID {productDescriptionId} non trovato.");
            }

            // Aggiorna i campi di productModel
            productModel.Name = productModelDescriptionDTO.Name;
#pragma warning disable CS8604 // Possibile argomento di riferimento Null.
            productModel.CatalogDescription = ConvertStringToXml(productModelDescriptionDTO.CatalogDescription, productModelDescriptionDTO.ProductModelId, productModelDescriptionDTO.Name);
#pragma warning restore CS8604 // Possibile argomento di riferimento Null.

            // Aggiorna i campi di productDescription
            productDescription.Description = productModelDescriptionDTO.Description;
            productDescription.ModifiedDate = DateTime.Now; // o usa productModelDescriptionDTO.ModifiedDate se necessario

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Successo, nessun contenuto da restituire
            }
            catch (Exception ex)
            {
                // Log dell'errore
                _logger.LogError($"Errore durante l'aggiornamento: {ex.Message}");
                return StatusCode(500, "Errore interno del server durante l'aggiornamento.");
            }
        }

        private string ConvertStringToXml(string catalogDescription, int productModelId, string productName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(catalogDescription))
                {
                    return string.Empty;
                }

                var xml = $@"<?xml-stylesheet href='ProductDescription.xsl' type='text/xsl'?>
<ProductDescription xmlns='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelDescription'
                    xmlns:xhtml='http://www.w3.org/1999/xhtml'
                    ProductModelID='{productModelId}'
                    ProductModelName='{System.Security.SecurityElement.Escape(productName)}'>
    <Summary>
        <xhtml:p>{System.Security.SecurityElement.Escape(catalogDescription)}</xhtml:p>
    </Summary>
    <!-- Aggiungi altri elementi XML conformi allo schema qui -->
</ProductDescription>";

                return xml;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante la conversione della stringa in XML: {ex.Message}");
                return string.Empty; // o gestisci l'errore come preferisci
            }
        }


        // POST: api/ProductModelProductDescriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult UpdateProductModelDescription([FromBody] ProductModelDescriptionDTO productModelDescriptionDTO)
        {
            if (productModelDescriptionDTO == null)
            {
                _logger.LogError("UpdateProductModelDescription chiamato con DTO null.");
                return BadRequest("Il DTO non può essere null.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var productModel = _context.ProductModels.FirstOrDefault(pm => pm.ProductModelId == productModelDescriptionDTO.ProductModelId);
                    var productDescription = _context.ProductDescriptions.FirstOrDefault(pd => pd.ProductDescriptionId == productModelDescriptionDTO.ProductDescriptionId);

                    _logger.LogInformation("Inizio aggiornamento ProductModelDescription");

                    var newGuid = Guid.NewGuid();

                    // Creazione o aggiornamento del ProductModel
                    if (productModel == null)
                    {
                        var catalogDescriptionXml = CreateCatalogDescriptionXml(productModelDescriptionDTO);

                        productModel = new ProductModel
                        {
                            ProductModelId = productModelDescriptionDTO.ProductModelId,
                            Name = productModelDescriptionDTO.Name,
                            CatalogDescription = catalogDescriptionXml,
                            Rowguid = newGuid
                        };
                        _context.ProductModels.Add(productModel);
                    }
                    else
                    {
                        var catalogDescriptionXml = CreateCatalogDescriptionXml(productModelDescriptionDTO);

                        productModel.Name = productModelDescriptionDTO.Name;
                        productModel.CatalogDescription = catalogDescriptionXml;
                        productModel.Rowguid = newGuid;
                    }

                    // Creazione o aggiornamento del ProductDescription
                    if (productDescription == null)
                    {
                        productDescription = new ProductDescription
                        {
                            ProductDescriptionId = productModelDescriptionDTO.ProductDescriptionId,
                            Description = productModelDescriptionDTO.Description,
                            ModifiedDate = DateTime.Now,
                            Rowguid = newGuid
                        };
                        _context.ProductDescriptions.Add(productDescription);
                    }
                    else
                    {
                        productDescription.Description = productModelDescriptionDTO.Description;
                        productDescription.ModifiedDate = DateTime.Now;
                        productDescription.Rowguid = newGuid;
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    _logger.LogInformation("ProductModelDescription aggiornato con successo");

                    return Ok("Record aggiornati con successo");
                }
                catch (Exception ex)
                {
                    // Log dell'errore principale
                    _logger.LogError(ex, "Errore durante l'aggiornamento di ProductModelDescription: " + ex.Message);

                    // Verifica se c'è un'inner exception e la registra
                    if (ex.InnerException != null)
                    {
                        _logger.LogError(ex.InnerException, V + ex.InnerException.Message);
                    }

                    transaction.Rollback();
                    return BadRequest($"Errore durante l'aggiornamento: {ex.Message}");
                }

            }
        }

        private string CreateCatalogDescriptionXml(ProductModelDescriptionDTO dto)
        {
            var xml = $@"<?xml-stylesheet href='ProductDescription.xsl' type='text/xsl'?>
    <ProductDescription xmlns='http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/ProductModelDescription'
                        xmlns:xhtml='http://www.w3.org/1999/xhtml'
                        ProductModelID='{dto.ProductModelId}'
                        ProductModelName='{System.Security.SecurityElement.Escape(dto.Name)}'>
        <Summary>
            <xhtml:p>{System.Security.SecurityElement.Escape(dto.CatalogDescription)}</xhtml:p>
        </Summary>
        <!-- Aggiungi altri elementi XML conformi allo schema qui -->
    </ProductDescription>";

            return xml;
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
