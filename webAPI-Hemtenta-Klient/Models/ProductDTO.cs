using System.ComponentModel;

namespace WebAPI_Hemtenta.Models
{
    public class ProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [Description("Image URL")]

        public string ImageUrl { get; set; }

        [Description("URL slug")]
        public string UrlSlug { get; set; }

        public int[] CategoriesId { get; set; }
        

       

    }
}
