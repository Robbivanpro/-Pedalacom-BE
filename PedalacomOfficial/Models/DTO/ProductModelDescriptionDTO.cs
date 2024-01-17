namespace PedalacomOfficial.Models.DTO
{
    public class ProductModelDescriptionDTO
    {
        //Product Model
        public int ProductModelId { get; set; }

        public string Name { get; set; } = null!;

        public string? CatalogDescription { get; set; }

        public Guid Rowguid { get; set; }

        //Product Description
        public int ProductDescriptionId { get; set; }


        public string Description { get; set; } = null!;


        public DateTime ModifiedDate { get; set; }
    }
}
