using PedalacomOfficial.Models;

namespace PedalacomOfficial.Repositories.Interface
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address); 
        Task<IEnumerable<Address>> GetAllAsync();

        Task<Address> GetByID(Guid id);

        Task<Address>UpdateAsync(Address address);
    }
}
