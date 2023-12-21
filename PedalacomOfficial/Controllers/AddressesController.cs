using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;
using PedalacomOfficial.Models.DTO;
using PedalacomOfficial.Repositories.Interface;
using PedalacomOfficial.Repositories.Implementation;

namespace PedalacomOfficial.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            
            this.addressRepository = addressRepository;
        }

        [HttpPost]

        public async Task<IActionResult> CreateAddresses(CreateAddressRequestDTO request)
        {
            var address = new Address
            {

                AddressLine1 = request.AddressLine1,


                AddressLine2 = request.AddressLine2,


                City = request.City,


                StateProvince = request.StateProvince,

                CountryRegion = request.CountryRegion,


                PostalCode =request.PostalCode,
            };

            await addressRepository.CreateAsync(address);

            var response = new AddressDTO
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                StateProvince = address.StateProvince,
                CountryRegion = address.CountryRegion,
                PostalCode = address.PostalCode,

            };

            return Ok(response);
        }

        // GET: api/Addresses

        [HttpGet]
        public async Task<IActionResult> GetAllAddress()
        {
            var addresses = await addressRepository.GetAllAsync();

            var response = new List <AddressDTO>();

            foreach (var address in addresses)
            {
                response.Add(new AddressDTO
                {
                    Id=address.Id,
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    StateProvince = address.StateProvince,
                    CountryRegion = address.CountryRegion,
                    PostalCode = address.PostalCode,
                });
            }
            return Ok(response);
        }

        // GET: api/Addresses/5
       [HttpGet]
       [Route("{id}")]

       public async Task<IActionResult> GetAddressById([FromRoute]Guid id)
        {
            var existingAddress = await addressRepository.GetByID(id);

            if(existingAddress is null)
            {
                return NotFound();
            }
            var response = new AddressDTO
            {
                Id = existingAddress.Id,
                AddressLine1 = existingAddress.AddressLine1,
                AddressLine2 = existingAddress.AddressLine2,
                City = existingAddress.City,
                StateProvince = existingAddress.StateProvince,
                CountryRegion = existingAddress.CountryRegion,
                PostalCode = existingAddress.PostalCode,
            };

            return Ok (response);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditAddress([FromRoute] Guid id, UpdateAddress request)
        {
            // Cerca prima l'indirizzo esistente nel database
            var existingAddress = await addressRepository.GetByID(id);
            if (existingAddress == null)
            {
                return NotFound();
            }

            // Aggiorna i campi dell'indirizzo esistente con i dati della richiesta
            existingAddress.AddressLine1 = request.AddressLine1;
            existingAddress.AddressLine2 = request.AddressLine2;
            existingAddress.City = request.City;
            existingAddress.StateProvince = request.StateProvince;
            existingAddress.CountryRegion = request.CountryRegion;
            existingAddress.PostalCode = request.PostalCode;

            // Salva le modifiche nel database
            var updatedAddress = await addressRepository.UpdateAsync(existingAddress);
            if (updatedAddress == null)
            {
                // Gestisci il caso in cui l'aggiornamento non va a buon fine
                return StatusCode(500, "Errore durante l'aggiornamento dell'indirizzo");
            }

            // Crea il DTO di risposta con i dati aggiornati
            var response = new AddressDTO
            {
                Id = updatedAddress.Id,
                AddressLine1 = updatedAddress.AddressLine1,
                AddressLine2 = updatedAddress.AddressLine2,
                City = updatedAddress.City,
                StateProvince = updatedAddress.StateProvince,
                CountryRegion = updatedAddress.CountryRegion,
                PostalCode = updatedAddress.PostalCode,
            };

            return Ok(response);
        }



        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (dbContext.Addresses == null)
            {
                return NotFound();
            }
            var address = await dbContext.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            dbContext.Addresses.Remove(address);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (dbContext.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
