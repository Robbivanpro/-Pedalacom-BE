using System.Text.Json.Serialization;

namespace PedalacomOfficial.Models.DTO
{
    public class ProductCategoryDTO
    {


        public int ProductCategoryId { get; set; }


        public int? ParentProductCategoryId { get; set; }


        public string Name { get; set; } = null!;

        [JsonIgnore]
        public Guid Rowguid { get; set; }


        public DateTime ModifiedDate { get; set; }



        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }

}

