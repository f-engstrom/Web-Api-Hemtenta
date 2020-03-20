using System.ComponentModel;

namespace WebAPI_Hemtenta.Models
{
    public class CreateProductDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        [Description("Image URL")]

        public string ImageUrl { get; set; }

     

        public int[] CategoriesId { get; set; }
    }
}