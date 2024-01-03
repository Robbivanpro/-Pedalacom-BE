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
    public class SalesOrderDetailsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly ILogger<SalesOrderDetailsController> _logger;
        public SalesOrderDetailsController(AdventureWorksLt2019Context context, ILogger<SalesOrderDetailsController> logger)
        {
            _context = context;
            _logger = logger;  
        }

        // GET: api/SalesOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderDetail>>> GetSalesOrderDetails()
        {
            try
            {
                _logger.LogInformation("Getting all sales order details");
                if (_context.SalesOrderDetails == null)
                {
                    _logger.LogWarning("SalesOrderDetails list is null");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting all sales order details: {ex.Message}");
            }
            return await _context.SalesOrderDetails.ToListAsync();
        }

        // GET: api/SalesOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDetail>> GetSalesOrderDetail(int id)
        {
            try
            {
                _logger.LogInformation($"Getting sales order detail with ID: {id}");
                if (_context.SalesOrderDetails == null)
                {
                    _logger.LogWarning("SalesOrderDetails list is null");
                    return NotFound();
                }
                var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);

                if (salesOrderDetail == null)
                {
                    _logger.LogWarning($"Sales order detail with ID {id} not found");
                    return NotFound();
                }

                return salesOrderDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting sales order detail with ID {id}: {ex.Message}");
            }
          return NotFound();
        }

        // PUT: api/SalesOrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderDetail(int id, SalesOrderDetail salesOrderDetail)
        {
            
            try
            {
                _logger.LogInformation($"Updating sales order detail with ID: {id}");
                if (id != salesOrderDetail.SalesOrderId)
                {
                    _logger.LogError("Bad request - ID mismatch");
                    return BadRequest();
                }

                _context.Entry(salesOrderDetail).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Concurrency exception while updating sales order detail with ID {id}: {ex.Message}");
                if (!SalesOrderDetailExists(id))
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
                _logger.LogError($"An error occurred while updating sales order detail with ID {id}: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/SalesOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrderDetail>> PostSalesOrderDetail(SalesOrderDetail salesOrderDetail)
        {
            try
            {
                _logger.LogInformation("Creating a new sales order detail");
                if (_context.SalesOrderDetails == null)
                {
                    _logger.LogWarning("SalesOrderDetails list is null");
                    return Problem("Entity set 'AdventureWorksLt2019Context.SalesOrderDetails'  is null.");
                }
                _context.SalesOrderDetails.Add(salesOrderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"A database update exception occurred while creating a new sales order detail: {ex.Message}");
                if (SalesOrderDetailExists(salesOrderDetail.SalesOrderId))
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
                _logger.LogError($"An error occurred while creating a new sales order detail: {ex.Message}");
            }
            return CreatedAtAction("GetSalesOrderDetail", new { id = salesOrderDetail.SalesOrderId }, salesOrderDetail);
        }

        // DELETE: api/SalesOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrderDetail(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting sales order detail with ID: {id}");
                if (_context.SalesOrderDetails == null)
                {
                    _logger.LogWarning("SalesOrderDetails list is null");
                    return NotFound();
                }
                var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);
                if (salesOrderDetail == null)
                {
                    _logger.LogWarning($"Sales order detail with ID {id} not found");
                    return NotFound();
                }

                _context.SalesOrderDetails.Remove(salesOrderDetail);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting sales order detail with ID {id}: {ex.Message}");
            }
            
            return NoContent();
        }

        private bool SalesOrderDetailExists(int id)
        {
            return (_context.SalesOrderDetails?.Any(e => e.SalesOrderId == id)).GetValueOrDefault();
        }
    }
}
