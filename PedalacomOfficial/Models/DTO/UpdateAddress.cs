namespace PedalacomOfficial.Models.DTO
{
    public class UpdateAddress
    {
        public int AddressId { get; set; }


        public string AddressLine1 { get; set; } = null!;


        public string? AddressLine2 { get; set; }


        public string City { get; set; } = null!;


        public string StateProvince { get; set; } = null!;

        public string CountryRegion { get; set; } = null!;


        public string PostalCode { get; set; } = null!;

    }
}
