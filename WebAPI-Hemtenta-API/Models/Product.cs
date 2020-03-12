using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_Web_API_Kurs.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        public string UrlSlug { get; set; }

        [JsonIgnore]
        public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

      
    }
}
