using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_Web_API_Kurs.Models.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        public string UrlSlug { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();



    }
}
