namespace PedalacomOfficial.Models.DTO
{
    public class AddressDTO
    {

        public Guid Id { get; set; }

        public string AddressLine1 { get; set; }


        public string? AddressLine2 { get; set; }


        public string City { get; set; } = null!;


        public string StateProvince { get; set; } = null!;

        public string CountryRegion { get; set; } = null!;


        public string PostalCode { get; set; } 


    }
}
