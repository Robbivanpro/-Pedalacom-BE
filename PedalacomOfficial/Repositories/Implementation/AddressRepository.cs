using Microsoft.EntityFrameworkCore;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;
using PedalacomOfficial.Repositories.Interface;

namespace PedalacomOfficial.Repositories.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AdventureWorksLt2019Context dbContext;

        public AddressRepository(AdventureWorksLt2019Context dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Address> CreateAsync(Address address)
        {
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();

            return address;
            
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await dbContext.Addresses.ToListAsync();
        }

        public async Task<Address?>GetByID(Guid id)
        {
            return await dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Address?> UpdateAsync(Address address)
        {
            var existingAdress = await dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id);

            if (existingAdress != null)
            {
                dbContext.Entry(existingAdress).CurrentValues.SetValues(address);
                await dbContext.SaveChangesAsync();
                return existingAdress;
            }
            return null;
        }
    }
}
