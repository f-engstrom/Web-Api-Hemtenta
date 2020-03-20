using System.ComponentModel;

namespace API_Web_API_Kurs.Models.DTO
{
    public class CreateProductDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [Description("Image URL")]

        public string ImageUrl { get; set; }


        public int[] CategoriesId { get; set; }
    }
}