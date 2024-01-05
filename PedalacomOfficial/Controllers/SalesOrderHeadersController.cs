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
    public class SalesOrderHeadersController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<SalesOrderHeadersController> _logger;
        public SalesOrderHeadersController(AdventureWorksLt2019Context context, ILogger<SalesOrderHeadersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/SalesOrderHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderHeader>>> GetSalesOrderHeaders()
        {
            try
            {
                _logger.LogInformation("Getting all sales order headers");
                if (_context.SalesOrderHeaders == null)
                {
                    _logger.LogWarning("SalesOrderHeaders list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all sales order headers: {ex.Message}");
            }
          
            return await _context.SalesOrderHeaders.ToListAsync();
        }

        // GET: api/SalesOrderHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderHeader>> GetSalesOrderHeader(int id)
        {
            try
            {
                _logger.LogInformation($"Getting sales order header with ID: {id}");
                if (_context.SalesOrderHeaders == null)
                {
                    _logger.LogWarning("SalesOrderHeaders list is null");
                    return NotFound();
                }
                var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);

                if (salesOrderHeader == null)
                {
                    _logger.LogWarning($"Sales order header with ID {id} not found");
                    return NotFound();
                }

                return salesOrderHeader;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting sales order header with ID {id}: {ex.Message}");
            }
          return NotFound();
        }

        // PUT: api/SalesOrderHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderHeader(int id, SalesOrderHeader salesOrderHeader)
        {
            
            try
            {
                _logger.LogInformation($"Updating sales order header with ID: {id}");
                if (id != salesOrderHeader.SalesOrderId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(salesOrderHeader).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!SalesOrderHeaderExists(id))
                {
                    _logger.LogError($"Concurrency exception while updating sales order header with ID {id}: {ex.Message}");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating sales order header with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/SalesOrderHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrderHeader>> PostSalesOrderHeader(SalesOrderHeader salesOrderHeader)
        {
            try
            {
                _logger.LogInformation("Creating a new sales order header");
                if (_context.SalesOrderHeaders == null)
                {
                    _logger.LogWarning("SalesOrderHeaders list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.SalesOrderHeaders'  is null.");
                }
                _context.SalesOrderHeaders.Add(salesOrderHeader);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a new sales order header: {ex.Message}");
            }
            
          
            return CreatedAtAction("GetSalesOrderHeader", new { id = salesOrderHeader.SalesOrderId }, salesOrderHeader);
        }

        // DELETE: api/SalesOrderHeaders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrderHeader(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting sales order header with ID: {id}");
                if (_context.SalesOrderHeaders == null)
                {
                    _logger.LogWarning("SalesOrderHeaders list is null");
                    return NotFound();
                }
                var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);
                if (salesOrderHeader == null)
                {
                    _logger.LogWarning($"Sales order header with ID {id} not found");
                    return NotFound();
                }

                _context.SalesOrderHeaders.Remove(salesOrderHeader);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting sales order header with ID {id}: {ex.Message}");
            }
       
            return NoContent();
        }

        private bool SalesOrderHeaderExists(int id)
        {
            return (_context.SalesOrderHeaders?.Any(e => e.SalesOrderId == id)).GetValueOrDefault();
        }
    }
}
