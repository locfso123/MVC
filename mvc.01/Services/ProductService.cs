using mvc01.Models;

namespace mvc01.Services
{
    public class ProductService : List<ProductModel>
    {
        public ProductService()
        {
            this.AddRange(new ProductModel[] {
                new ProductModel{Id=1, Name="Milk", Price=10},
                new ProductModel{Id=2, Name="Egg", Price=20},
                new ProductModel{Id=3, Name="Bread", Price=30},
            });
        }
    }
}
