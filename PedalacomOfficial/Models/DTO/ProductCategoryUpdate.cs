namespace PedalacomOfficial.Models.DTO
{
    public class ProductCategoryUpdate
    {

        public int ProductCategoryId { get; set; }


        


        public string Name { get; set; } = null!;


       


        public DateTime ModifiedDate { get; set; }



        public virtual ICollection<Product> Products { get; } = new List<Product>();
    }
}
